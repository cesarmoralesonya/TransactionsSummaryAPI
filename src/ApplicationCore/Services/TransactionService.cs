using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Specifications;
using Infraestructure.Interfaces;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionClient<TransactionModel> _transactionClient;


        public TransactionService(IMapper mapper, ITransactionRepository transactionRepository, ITransactionClient<TransactionModel> transactionClient)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _transactionClient = transactionClient ?? throw new ArgumentNullException(nameof(transactionClient));
        }

        public async Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync()
        {
            var transactions = await _transactionClient.GetAll();
            if (transactions == null)
            {
                var transPersisted = await _transactionRepository.ListAllAsync();
                return _mapper.Map<IEnumerable<TransactionDto>>(transPersisted);
            }
            else
            {
                await UpdateAllPersistedTransactions(transactions);
                return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
            }
        }

        public async Task<TransactionsTotalDto> GetTransactionsWithTotal(string sku)
        {
            var filterSpec = new TransactionsFilterSpecification(sku);
            var transPersisFiltered = await _transactionRepository.ListAsync(filterSpec);
            return new TransactionsTotalDto()
            {
                Transactions = _mapper.Map<List<TransactionDto>>(transPersisFiltered),
                Total = transPersisFiltered.Select(trans => trans.Amount).Sum(),
            };
        }

        private static double GetExchangeRate(string from, string to, double amount = 1)
        {
            if (from == null || to == null) return 0;

            if (from.ToLower() == "eur" && to.ToLower() == "eur")
                return amount;

            // First Get the exchange rate of both currencies in euro
            double toRate = GetCurrencyRateInEuro(to);
            double fromRate = GetCurrencyRateInEuro(from);

            // Convert Between Euro to Other Currency
            if (from.ToLower() == "eur")
            {
                return (amount * toRate);
            }
            else if (to.ToLower() == "eur")
            {
                return (amount / fromRate);
            }
            else
            {
                // Calculate non EURO exchange rates From A to B
                return (amount * toRate) / fromRate;
            }
        }

        private static double GetCurrencyRateInEuro(string currency)
        {
            return 1;
        }

        private async Task UpdateAllPersistedTransactions(IEnumerable<TransactionModel> transactions)
        {
            var transactionEntities = _mapper.Map<IEnumerable<TransactionEntity>>(transactions);
            await _transactionRepository.DeleteAllAsync();
            await _transactionRepository.AddRangeAsync(transactionEntities);
        }
    }
}

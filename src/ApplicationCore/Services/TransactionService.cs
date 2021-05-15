
using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Specifications;
using Infraestructure.Interfaces;
using Infraestructure.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionClient<TransactionModel> _transactionClient;
        private readonly ILogger _logger;


        public TransactionService(IMapper mapper,
                                    ITransactionRepository transactionRepository,
                                    ITransactionClient<TransactionModel> transactionClient,
                                    ILogger<TransactionService> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _transactionClient = transactionClient ?? throw new ArgumentNullException(nameof(transactionClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync(CancellationToken cancellationToken = default)
        {
            var transactions = await _transactionClient.GetAllAsync(cancellationToken);
            if (transactions == null)
            {
                _logger.LogWarning($"Client {nameof(_transactionClient)} unavailable return {nameof(transactions)}");
                var transPersisted = await _transactionRepository.ListAllAsync(cancellationToken);
                if(transPersisted == null)
                    throw new ArgumentException($"{nameof(transPersisted)} is null. Can not return data");
                return _mapper.Map<IEnumerable<TransactionDto>>(transPersisted);
            }
            else
            {
                //await UpdatePersistedTransactions(transactions);
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

        private async Task UpdatePersistedTransactions(IEnumerable<TransactionModel> transactions, CancellationToken cancellationToken = default)
        {
            var transactionEntities = _mapper.Map<IEnumerable<TransactionEntity>>(transactions);
            await _transactionRepository.DeleteAllAsync(cancellationToken);
            await _transactionRepository.AddRangeAsync(transactionEntities, cancellationToken);
        }
    }
}

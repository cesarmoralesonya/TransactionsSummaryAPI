using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Services.Dtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionClient<TransactionModel> _transactionClient;


        public TransactionService(IMapper mapper, ITransactionRepository transactionRepository, ITransactionClient<TransactionModel> transactionClient)
        {
            _mapper  = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _transactionClient = transactionClient ?? throw new ArgumentNullException(nameof(transactionClient));
        }

        public async Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync()
        {
            var transactions = await _transactionClient.GetAll();
            if(transactions == null)
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

        private async Task UpdateAllPersistedTransactions(IEnumerable<TransactionModel> transactions)
        {
            var transactionEntities = _mapper.Map<IEnumerable<TransactionEntity>>(transactions);
            await _transactionRepository.DeleteAllAsync();
            await _transactionRepository.AddRangeAsync(transactionEntities);
        }
    }
}

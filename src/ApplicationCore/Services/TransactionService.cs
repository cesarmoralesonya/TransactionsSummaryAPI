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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionClient<TransactionModel> _transactionClient;


        public TransactionService(IMapper mapper, IUnitOfWork unitOfWork, ITransactionClient<TransactionModel> transactionClient)
        {
            _mapper  = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _transactionClient = transactionClient ?? throw new ArgumentNullException(nameof(transactionClient));
        }

        public async Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync()
        {
            var transactions = await _transactionClient.GetAll();
            if(transactions == null)
            {
                var transPersisted = _unitOfWork.Transactions.ListAllAsync();
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
            var transactionsEntity = _mapper.Map<IEnumerable<TransactionEntity>>(transactions);
            await _unitOfWork.Transactions.DeleteAllAsync();
            await _unitOfWork.Transactions.AddRangeAsync(transactionsEntity);
            await _unitOfWork.CommitAsync();
        }
    }
}

using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Specifications;
using Infraestructure.Interfaces;
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
        private readonly ITransactionClient _transactionClient;
        private readonly ILogger _logger;


        public TransactionService(IMapper mapper,
                                    ITransactionRepository transactionRepository,
                                    ITransactionClient transactionClient,
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
                var transBackup = await _transactionRepository.ListAllAsync(cancellationToken);
                if (transBackup == null)
                    throw new ArgumentException($"{nameof(transBackup)} is null. Can not return data");
                return _mapper.Map<IEnumerable<TransactionDto>>(transBackup);
            }
            else
            {
                var transEntities = _mapper.Map<IEnumerable<TransactionEntity>>(transactions);
                await _transactionRepository.UpdateBackupAsync(transEntities, cancellationToken);
                return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
            }
        }
    }
}

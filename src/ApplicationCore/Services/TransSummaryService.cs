using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
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
    public class TransSummaryService : ITransSummaryService
    {
        private readonly IMapper _mapper;

        private readonly IRateClient _rateClient;
        private readonly IRateRepository _rateRepository;

        private readonly ITransactionClient _transactionClient;
        private readonly ITransactionRepository _transactionRepository;

        private readonly ILogger _logger;

        public TransSummaryService(IMapper mapper,
                                    IRateClient rateClient,
                                    IRateRepository rateRepository,
                                    ITransactionClient transactionClient,
                                    ITransactionRepository transactionRepository, 
                                    ILogger<TransSummaryService> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _rateClient = rateClient ?? throw new ArgumentNullException(nameof(rateClient));
            _rateRepository = rateRepository ?? throw new ArgumentNullException(nameof(rateRepository));
            _transactionClient = transactionClient ?? throw new ArgumentNullException(nameof(transactionClient));
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TransactionsTotalDto> GetTransactionsWithTotal(string sku, CancellationToken cancellationToken = default)
        {
            var transactions = await _transactionClient.GetAllAsync(cancellationToken);
            var transactionsBySkuApi = transactions.Where(trans => trans.Sku == sku).ToList();
            var transactionsBySkyDto = new List<TransactionDto>();
            
            if(!transactionsBySkuApi.Any())
            {
                _logger.LogWarning($"Client {nameof(_transactionClient)} not return a list by sku");
                var filterSpec = new TransactionsFilterSpecification(sku);
                var transactionsBySkuBackup = await _transactionRepository.ListAsync(filterSpec);
                if(transactionsBySkuBackup == null)
                    throw new ArgumentException($"{nameof(transactionsBySkuBackup)} is null. Can not return data");
                transactionsBySkyDto = _mapper.Map<List<TransactionDto>>(transactionsBySkuBackup);
            }
            else
            {
                transactionsBySkyDto = _mapper.Map<List<TransactionDto>>(transactionsBySkuApi);
            }

            if(transactionsBySkyDto.Any())
            {
                foreach (var transaction in transactionsBySkyDto)
                {
                    transaction.Amount = await ExchangeToEur(transaction.Currency, transaction.Amount);
                    transaction.Currency = "EUR";
                }

                return new TransactionsTotalDto()
                {
                    Transactions = transactionsBySkyDto,
                    Total = decimal.Round(transactionsBySkyDto.Sum(transaction => transaction.Amount),2)
                };
            }
            else
            {
                return new TransactionsTotalDto()
                {
                    Transactions = new List<TransactionDto>(),
                    Total = 0
                };
            }
        }

        private async Task<decimal> ExchangeToEur(string from, decimal amount, CancellationToken cancellationToken = default)
        {
            //Get rates:
            var rates = new List<RateDto>();
            var ratesApi = await _rateClient.GetAllAsync(cancellationToken);
            if(ratesApi == null)
            {
                var ratesBackup = await _rateRepository.ListAllAsync();
                rates = _mapper.Map<List<RateDto>>(ratesBackup);
            }
            else
            {
                rates = _mapper.Map<List<RateDto>>(ratesApi);
            }

            if (!rates.Any())
                throw (new ArgumentException("Imposible do the Exchange because rates is empty"));

            //Exchange
            if (from.ToLower() == "eur") return amount;
            var ratesFromEur = rates.Where(rate => rate.From.ToLower() == "eur").ToList();
            if (ratesFromEur.Exists(rate => rate.To.ToLower() == from.ToLower()))
            {
                var rateExchange = ratesFromEur
                                    .Where(rateFromEur => rateFromEur.To.ToLower() == from.ToLower())
                                    .Select(rateFromEur => rateFromEur.Rate)
                                    .FirstOrDefault();
                return amount / rateExchange;
            }
            else
            {
                var ratesFromOther = rates.Where(rate => rate.From.ToLower() == from.ToLower()).ToList();
                var rateFromMatch = ratesFromEur
                                    .Where(rateFromEur => 
                                            ratesFromOther
                                            .Any(rateFromOther => rateFromEur.To == rateFromOther.To))
                                    .FirstOrDefault();

                var rateToMach = ratesFromEur
                                    .Where(rateFromEur => rateFromEur.To == rateFromMatch.To)
                                    .FirstOrDefault();

                return amount * rateFromMatch.Rate / rateToMach.Rate;
            }
        }
    }
}

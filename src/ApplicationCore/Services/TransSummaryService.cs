﻿using Application.Dtos;
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

        private Dictionary<string, List<string>> _graph;


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

        private async Task<decimal> ExchangeToEur(string currency, decimal amount, CancellationToken cancellationToken = default)
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

            ConstructGraph(rates);
            return amount * ExchangeRate(currency, "EUR", rates);
        }

        private void ConstructGraph(List<RateDto> rates)
        {
            if (_graph == null)
            {
                _graph = new Dictionary<string, List<string>>();
                foreach (var rate in rates)
                {
                    if (!_graph.ContainsKey(rate.From))
                        _graph[rate.From] = new List<string>();
                    if (!_graph.ContainsKey(rate.To))
                        _graph[rate.To] = new List<string>();

                    _graph[rate.From].Add(rate.To);
                    _graph[rate.To].Add(rate.From);
                }
            }
        }

        private decimal ExchangeRate(string baseCode, string targetCode, List<RateDto> rates)
        {
            if (_graph[baseCode].Contains(targetCode))
            {
                // found the target code
                return GetKnownRate(baseCode, targetCode, rates);
            }
            else
            {
                foreach (var code in _graph[baseCode])
                {
                    // determine if code can be converted to targetCode
                    decimal rate = ExchangeRate(code, targetCode, rates);
                    if (rate != 0) // if it can than combine with returned rate
                        return rate*GetKnownRate(baseCode, code, rates);
                }
            }

            return 0;
        }
        private decimal GetKnownRate(string baseCode, string targetCode, List<RateDto> rates)
        {
            //Calculate not knowledge rates
            for (int i = 0; i < rates.Count; i++)
            {
                RateDto rateDto = rates[i];
                for (int j = i + 1; j < rates.Count; j++)
                {
                    RateDto rate2 = rates[j];
                    RateDto cross = CanCross(rateDto, rate2);
                    if (cross != null)
                        if (rates.FirstOrDefault(r => r.From.Equals(cross.From) && r.To.Equals(cross.To)) == null)
                            rates.Add(cross);
                }
            }

            var rate = rates.SingleOrDefault(fr => fr.From == baseCode && fr.To == targetCode);
            var rate_i = rates.SingleOrDefault(fr => fr.From == targetCode && fr.To == baseCode);
            
            if (rate == null)
            {
                return 1 / rate_i.Rate;
            }
            return rate.Rate;
        }

        public static RateDto CanCross(RateDto r1, RateDto r2)
        {
            RateDto nr = null;

            if (r1.From.Equals(r2.From) && r1.To.Equals(r2.To) ||
                r1.From.Equals(r2.To) && r1.To.Equals(r2.From)
                ) return null; // Same with same.

            if (r1.From.Equals(r2.From))
            { // a/b / a/c = c/b
                nr = new RateDto()
                {
                    From = r2.To,
                    To = r1.To,
                    Rate = r1.Rate / r2.Rate
                };
            }
            else if (r1.From.Equals(r2.To))
            {
                // a/b * c/a = c/b
                nr = new RateDto()
                {
                    From = r2.From,
                    To = r1.To,
                    Rate = r2.Rate * r1.Rate
                };
            }
            else if (r1.To.Equals(r2.To))
            {
                // a/c / b/c = a/b
                nr = new RateDto()
                {
                    From = r1.From,
                    To = r2.From,
                    Rate = r1.Rate / r2.Rate
                };
            }
            else if (r1.To.Equals(r2.From))
            {
                // a/c * c/b = a/b
                nr = new RateDto()
                {
                    From = r1.From,
                    To = r2.To,
                    Rate = r1.Rate * r2.Rate
                };
            }
            return nr;
        }
    }
}

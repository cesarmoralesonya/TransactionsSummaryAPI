using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Infraestructure.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private readonly IMapper _mapper;

        private readonly IRateClient _rateClient;
        private readonly IRateRepository _rateRepository;

        private Dictionary<string, List<string>> _graph;
        private List<RateDto> _rates;

        private readonly ILogger _logger;


        public CurrencyConverterService(IMapper mapper,
                                        IRateClient rateClient,
                                        IRateRepository rateRepository,
                                        ILogger<CurrencyConverterService> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _rateClient = rateClient ?? throw new ArgumentNullException(nameof(rateClient));
            _rateRepository = rateRepository ?? throw new ArgumentNullException(nameof(rateRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            Initialization = InitializeAsync();

        }

        public Task Initialization { get; private set; }

        private async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            var ratesApi = await _rateClient.GetAllAsync(cancellationToken);
            if (ratesApi == null)
            {
                var ratesBackup = await _rateRepository.ListAllAsync();
                _rates = _mapper.Map<List<RateDto>>(ratesBackup);
            }
            else
            {
                _rates = _mapper.Map<List<RateDto>>(ratesApi);
            }

            if (!_rates.Any())
                throw (new ArgumentException("Imposible do the Exchange because rates is empty"));

            //If rates contains data:
            ConstructGraph();

            //Add unknown rates to rates list
            CalculateUnknownRates();
        }

        private void ConstructGraph()
        {
            if (_graph == null)
            {
                _graph = new Dictionary<string, List<string>>();
                foreach (var rate in _rates)
                {
                    if (!_graph.ContainsKey(rate.From))
                        _graph[rate.From] = new List<string>();
                    if (!_graph.ContainsKey(rate.To))
                        _graph[rate.To] = new List<string>();

                    _graph[rate.From].Add(rate.To);
                    _graph[rate.To].Add(rate.From);
                }
                _logger.LogInformation("Graph Constructed");
            }
        }

        private void CalculateUnknownRates()
        {
            for (int i = 0; i < _rates.Count; i++)
            {
                RateDto rateFile = _rates[i];
                for (int j = i + 1; j < _rates.Count; j++)
                {
                    RateDto rateRow = _rates[j];
                    RateDto cross = CrossRate(rateFile, rateRow);
                    if (cross != null)
                        if (_rates.FirstOrDefault(r => r.From.Equals(cross.From) && r.To.Equals(cross.To)) == null)
                            _rates.Add(cross);
                }
            }
        }

        private RateDto CrossRate(RateDto rateMother, RateDto rateFather)
        {
            RateDto rateSon = null;

            if (rateMother.From.Equals(rateFather.From) && rateMother.To.Equals(rateFather.To) ||
                rateMother.From.Equals(rateFather.To) && rateMother.To.Equals(rateFather.From)
                ) return null; // Same with same.

            if (rateMother.From.Equals(rateFather.From))
            { // a/b / a/c = c/b
                rateSon = new RateDto()
                {
                    From = rateFather.To,
                    To = rateMother.To,
                    Rate = rateMother.Rate / rateFather.Rate
                };
            }
            else if (rateMother.From.Equals(rateFather.To))
            {
                // a/b * c/a = c/b
                rateSon = new RateDto()
                {
                    From = rateFather.From,
                    To = rateMother.To,
                    Rate = rateFather.Rate * rateMother.Rate
                };
            }
            else if (rateMother.To.Equals(rateFather.To))
            {
                // a/c / b/c = a/b
                rateSon = new RateDto()
                {
                    From = rateMother.From,
                    To = rateFather.From,
                    Rate = rateMother.Rate / rateFather.Rate
                };
            }
            else if (rateMother.To.Equals(rateFather.From))
            {
                // a/c * c/b = a/b
                rateSon = new RateDto()
                {
                    From = rateMother.From,
                    To = rateFather.To,
                    Rate = rateMother.Rate * rateFather.Rate
                };
            }
            return rateSon;
        }

        public decimal ExchangeRate(string baseCode, string targetCode)
        {
            if (_graph[baseCode].Contains(targetCode))
            {
                // found the target code
                return GetRate(baseCode, targetCode);
            }
            else
            {
                foreach (var code in _graph[baseCode])
                {
                    // determine if code can be converted to targetCode
                    decimal rate = ExchangeRate(code, targetCode);
                    if (rate != 0) // if it can than combine with returned rate
                        return rate * GetRate(baseCode, code);
                }
            }

            return 0;
        }

        private decimal GetRate(string baseCode, string targetCode)
        {
            var rate = _rates.SingleOrDefault(fr => fr.From == baseCode && fr.To == targetCode);
            var rate_i = _rates.SingleOrDefault(fr => fr.From == targetCode && fr.To == baseCode);

            if (rate == null)
            {
                return 1 / rate_i.Rate;
            }
            return rate.Rate;
        }
    }
}

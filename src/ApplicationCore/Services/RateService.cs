using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infraestructure.Interfaces;
using Infraestructure.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RateService : IRateService
    {
        private readonly IMapper _mapper;
        private readonly IRateRepository _rateRepository;
        private readonly IrateClient<RateModel> _rateClient;
        private readonly ILogger _logger;


        public RateService(IMapper mapper, IRateRepository rateRepository,
                                    IrateClient<RateModel> rateClient,
                                    ILogger<RateService> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _rateRepository = rateRepository ?? throw new ArgumentNullException(nameof(rateRepository));
            _rateClient = rateClient ?? throw new ArgumentNullException(nameof(rateClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<RateDto>> GetAllratesAsync(CancellationToken cancellationToken = default)
        {
            var rates = await _rateClient.GetAllAsync(cancellationToken);
            if (rates == null)
            {
                _logger.LogWarning($"Client {nameof(_rateClient)} unavailable return {nameof(rates)}");
                var convPersisted = await _rateRepository.ListAllAsync(cancellationToken);
                if (convPersisted == null)
                    throw new ArgumentException($"{nameof(convPersisted)} is null. Can not return data");
                return _mapper.Map<IEnumerable<RateDto>>(convPersisted);
            }
            else
            {
                await UpdatePersistedrates(rates, cancellationToken);
                return _mapper.Map<IEnumerable<RateDto>>(rates);
            }
        }

        private async Task UpdatePersistedrates(IEnumerable<RateModel> rates, CancellationToken cancellationToken = default)
        {
            var rateEntities = _mapper.Map<IEnumerable<RateEntity>>(rates);
            await _rateRepository.DeleteAllAsync(cancellationToken);
            await _rateRepository.AddRangeAsync(rateEntities);            
        }
    }
}

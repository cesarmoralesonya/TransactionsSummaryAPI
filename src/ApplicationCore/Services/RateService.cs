using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infraestructure.Interfaces;
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
        private readonly IRateClient _rateClient;
        private readonly ILogger _logger;


        public RateService(IMapper mapper, IRateRepository rateRepository,
                                    IRateClient rateClient,
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
                var rateBackup = await _rateRepository.ListAllAsync(cancellationToken);
                if (rateBackup == null)
                    throw new ArgumentException($"{nameof(rateBackup)} is null. Can not return data");
                return _mapper.Map<IEnumerable<RateDto>>(rateBackup);
            }
            else
            {
                var rateEntities = _mapper.Map<IEnumerable<RateEntity>>(rates);
                await _rateRepository.UpdateBackupAsync(rateEntities, cancellationToken);
                return _mapper.Map<IEnumerable<RateDto>>(rates);
            }
        }
    }
}

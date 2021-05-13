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
    public class ConversionService : IConversionService
    {
        private readonly IMapper _mapper;
        private readonly IConversionRepository _conversionRepository;
        private readonly IConversionClient<ConversionModel> _conversionClient;
        private readonly ILogger _logger;


        public ConversionService(IMapper mapper, IConversionRepository conversionRepository,
                                    IConversionClient<ConversionModel> conversionClient,
                                    ILogger<ConversionService> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _conversionRepository = conversionRepository ?? throw new ArgumentNullException(nameof(conversionRepository));
            _conversionClient = conversionClient ?? throw new ArgumentNullException(nameof(conversionClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ConversionDto>> GetAllConversionsAsync(CancellationToken cancellationToken = default)
        {
            var conversions = await _conversionClient.GetAllAsync(cancellationToken);
            if (conversions == null)
            {
                _logger.LogWarning($"Client {nameof(_conversionClient)} unavailable return {nameof(conversions)}");
                var convPersisted = await _conversionRepository.ListAllAsync(cancellationToken);
                if (convPersisted == null)
                    throw new ArgumentException($"{nameof(convPersisted)} is null. Can not return data");
                return _mapper.Map<IEnumerable<ConversionDto>>(convPersisted);
            }
            else
            {
                await UpdatePersistedConversions(conversions, cancellationToken);
                return _mapper.Map<IEnumerable<ConversionDto>>(conversions);
            }
        }

        private async Task UpdatePersistedConversions(IEnumerable<ConversionModel> conversions, CancellationToken cancellationToken = default)
        {
            var conversionEntities = _mapper.Map<IEnumerable<ConversionEntity>>(conversions);
            await _conversionRepository.DeleteAllAsync(cancellationToken);
            await _conversionRepository.AddRangeAsync(conversionEntities);            
        }
    }
}

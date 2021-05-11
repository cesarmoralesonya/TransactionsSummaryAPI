using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using PublicApi.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ConversionService : IConversionService
    {
        private readonly IMapper _mapper;
        private readonly IConversionRepository _conversionRepository;
        private readonly IConversionClient<ConversionModel> _conversionClient;


        public ConversionService(IMapper mapper, IConversionRepository conversionRepository, IConversionClient<ConversionModel> conversionClient)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _conversionRepository = conversionRepository ?? throw new ArgumentNullException(nameof(conversionRepository));
            _conversionClient = conversionClient ?? throw new ArgumentNullException(nameof(conversionClient));
        }

        public async Task<IEnumerable<ConversionDto>> GetAllConversionsAsync()
        {
            var conversions = await _conversionClient.GetAll();
            if (conversions == null)
            {
                var transPersisted = await _conversionRepository.ListAllAsync();
                return _mapper.Map<IEnumerable<ConversionDto>>(transPersisted);
            }
            else
            {
                await UpdateAllPersistedTransactions(conversions);
                return _mapper.Map<IEnumerable<ConversionDto>>(conversions);
            }
        }

        private async Task UpdateAllPersistedTransactions(IEnumerable<ConversionModel> conversions)
        {
            var conversionEntities = _mapper.Map<IEnumerable<ConversionEntity>>(conversions);
            await _conversionRepository.DeleteAllAsync();
            await _conversionRepository.AddRangeAsync(conversionEntities);
        }
    }
}

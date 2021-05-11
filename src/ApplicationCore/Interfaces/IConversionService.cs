using PublicApi.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IConversionService
    {
        Task<IEnumerable<ConversionDto>> GetAllConversionsAsync();
    }
}

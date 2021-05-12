using PublicApi.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IConversionService
    {
        Task<IEnumerable<ConversionDto>> GetAllConversionsAsync();
    }
}

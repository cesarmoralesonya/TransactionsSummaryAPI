using Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IConversionService
    {
        Task<IEnumerable<ConversionDto>> GetAllConversionsAsync();
    }
}

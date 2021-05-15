using Application.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRateService
    {
        Task<IEnumerable<RateDto>> GetAllratesAsync(CancellationToken cancellationToken = default);
    }
}

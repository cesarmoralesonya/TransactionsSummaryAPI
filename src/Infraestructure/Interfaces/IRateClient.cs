using Infraestructure.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infraestructure.Interfaces
{
    public interface IRateClient
    {
        Task<IEnumerable<RateModel>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}

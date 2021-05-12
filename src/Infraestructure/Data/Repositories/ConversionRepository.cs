using Domain.Entities;
using Infraestructure.Interfaces;

namespace Infraestructure.Data.Repositories
{
    public class ConversionRepository : EfRepository<ConversionEntity>, IConversionRepository
    {
        public ConversionRepository(TransSummaryContext dbcontext) : base(dbcontext)
        {
        }
    }
}

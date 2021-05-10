using ApplicationCore.Entities;
using ApplicationCore.Interfaces;

namespace Infraestructure.Data.Repositories
{
    public class ConversionRepository : EfRepository<ConversionDb>, IConversionRepository
    {
        public ConversionRepository(TransSummaryContext dbcontext) : base(dbcontext)
        {
        }
    }
}

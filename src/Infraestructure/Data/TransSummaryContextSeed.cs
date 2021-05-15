using Domain.Entities;
using System.Linq;

namespace Infraestructure.Data
{
    public static class TransSummaryContextSeed
    {
        public static void Initialize(TransSummaryContext context)
        {
            if (!context.rates.Any())
            {
                context.rates.AddRange(
                    new RateEntity("EUR", "USD", 1.359M),
                    new RateEntity("CAD", "EUR", 0.732M)
                    );
            }
            if (!context.Transactions.Any())
            {
                context.Transactions.AddRange(
                    new TransactionEntity("T2006", 10.00M, "USD"),
                    new TransactionEntity("R2008", 17.95M, "USD")
                    );
            }
            context.SaveChanges();
        }
    }
}

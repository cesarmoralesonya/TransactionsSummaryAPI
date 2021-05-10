using ApplicationCore.Entities;
using Infraestructure.Data.Config;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Data
{
    public class TransSummaryContext : DbContext
    {
        public TransSummaryContext([NotNull] DbContextOptions<TransSummaryContext> options) : base(options)
        {
        }

        public DbSet<ConversionEntity> Conversions { get; set; }
        public DbSet<TransactionEntity> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ConversionConfiguration());
            builder.ApplyConfiguration(new TransactionConfiguration());

        }
    }
}

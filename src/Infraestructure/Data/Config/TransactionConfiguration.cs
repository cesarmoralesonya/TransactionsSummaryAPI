using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Data.Config
{
    public class TransactionConfiguration : IEntityTypeConfiguration<TransactionDb>
    {
        public void Configure(EntityTypeBuilder<TransactionDb> builder)
        {
            builder.HasKey(c => c.Id);
            builder.ToTable("Transaction");
        }
    }
}

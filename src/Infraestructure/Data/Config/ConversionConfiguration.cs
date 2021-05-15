using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Data.Config
{
    public class RateConfiguration : IEntityTypeConfiguration<RateEntity>
    {
        public void Configure(EntityTypeBuilder<RateEntity> builder)
        {
            builder.HasKey(a => a.Id);
            builder.ToTable("rate");
        }
    }
}

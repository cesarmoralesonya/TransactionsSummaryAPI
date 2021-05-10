using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Data.Config
{
    public class ConversionConfiguration : IEntityTypeConfiguration<ConversionEntity>
    {
        public void Configure(EntityTypeBuilder<ConversionEntity> builder)
        {
            builder.HasKey(a => a.Id);
            builder.ToTable("Conversion");
        }
    }
}

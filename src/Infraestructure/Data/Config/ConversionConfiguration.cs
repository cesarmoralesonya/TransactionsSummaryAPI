using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Data.Config
{
    public class ConversionConfiguration : IEntityTypeConfiguration<ConversionDb>
    {
        public void Configure(EntityTypeBuilder<ConversionDb> builder)
        {
            builder.HasKey(a => a.Id);
            builder.ToTable("Conversion");
        }
    }
}

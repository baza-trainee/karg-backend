using karg.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace karg.DAL.Context.EntityConfigurations
{
    internal class YearResultConfiguration : IEntityTypeConfiguration<YearResult>
    {
        public void Configure(EntityTypeBuilder<YearResult> builder)
        {
            builder.ToTable("YearsResults");

            builder.HasKey(yearResult => yearResult.Id);

            builder.Property(yearResult => yearResult.Id).ValueGeneratedOnAdd();
            builder.Property(yearResult => yearResult.Created_At).IsRequired();
        }
    }
}

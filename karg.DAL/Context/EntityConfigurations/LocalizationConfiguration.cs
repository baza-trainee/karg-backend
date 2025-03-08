using karg.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace karg.DAL.Context.EntityConfigurations
{
    internal class LocalizationConfiguration : IEntityTypeConfiguration<Localization>
    {
        public void Configure(EntityTypeBuilder<Localization> builder)
        {
            builder.ToTable("Localizations");

            builder.HasKey(localization => new { localization.LocalizationSetId, localization.CultureCode });
            builder.Property(localization => localization.Value).HasMaxLength(10000).IsRequired();
        }
    }
}

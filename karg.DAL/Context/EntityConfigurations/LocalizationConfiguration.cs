using karg.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.DAL.Context.EntityConfigurations
{
    internal class LocalizationConfiguration : IEntityTypeConfiguration<Localization>
    {
        public void Configure(EntityTypeBuilder<Localization> builder)
        {
            builder.ToTable("Localization");

            builder.HasKey(localization => new { localization.LocalizationSetId, localization.CultureCode });
            builder.Property(localization => localization.Value).HasMaxLength(5000).IsRequired();
        }
    }
}

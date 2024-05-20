using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using karg.DAL.Models;

namespace karg.DAL.Context.EntityConfigurations
{
    internal class LocalizationSetConfiguration : IEntityTypeConfiguration<LocalizationSet>
    {
        public void Configure(EntityTypeBuilder<LocalizationSet> builder)
        {
            builder.ToTable("LocalizationSet");

            builder.HasKey(localizationSet => localizationSet.Id);
            builder.Property(localizationSet => localizationSet.Id).ValueGeneratedOnAdd();
        }
    }
}

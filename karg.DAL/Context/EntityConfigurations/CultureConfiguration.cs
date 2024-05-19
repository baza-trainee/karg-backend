using karg.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace karg.DAL.Context.EntityConfigurations
{
    internal class CultureConfiguration : IEntityTypeConfiguration<Culture>
    {
        public void Configure(EntityTypeBuilder<Culture> builder)
        {
            builder.ToTable("Culture");

            builder.HasKey(culture => culture.Code);
            builder.Property(culture => culture.Name).IsRequired().HasMaxLength(64);
        }
    }
}

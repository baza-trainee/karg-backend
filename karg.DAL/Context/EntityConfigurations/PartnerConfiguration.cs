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
    internal class PartnerConfiguration : IEntityTypeConfiguration<Partner>
    {
        public void Configure(EntityTypeBuilder<Partner> builder)
        {
            builder.ToTable("Partner");

            builder.HasKey(partner => partner.Id);

            builder.Property(partner => partner.Uri).IsRequired();
            builder.Property(partner => partner.Name).HasMaxLength(50).IsRequired();
        }
    }
}

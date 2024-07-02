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
    internal class JwtTokenConfiguration : IEntityTypeConfiguration<JwtToken>
    {
        public void Configure(EntityTypeBuilder<JwtToken> builder)
        {
            builder.ToTable("Token");

            builder.HasKey(token => token.Id);

            builder.Property(token => token.Id).ValueGeneratedOnAdd();
            builder.Property(token => token.Token).IsRequired();
        }
    }
}

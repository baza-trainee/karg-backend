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
            builder.ToTable("Tokens");

            builder.HasKey(token => token.Id);

            builder.Property(token => token.Id).ValueGeneratedOnAdd();
            builder.Property(token => token.Token).IsRequired();

            builder.HasData(
                new JwtToken { Id = 1, Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJGdWxsbmFtZSI6IkFkbWluIEtBUkciLCJSb2xlIjoiRGlyZWN0b3IiLCJFbWFpbCI6ImFkbWluQGdtYWlsLmNvbSIsImV4cCI6MzMyNTYxODA4OTEsImlzcyI6ImthcmcuY29tIiwiYXVkIjoia2FyZy5jb20ifQ.2RcVCXa9B_xS3zBEBTEAFsEyfS0DIpyWQtIBxs3IabM" });
        }
    }
}

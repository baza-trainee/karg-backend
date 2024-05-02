using karg.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using karg.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace karg.DAL.Context.EntityConfigurations
{
    internal class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            var contactCategoryConverter = new EnumToStringConverter<ContactCategory>();

            builder.ToTable("Contact");

            builder.HasKey(contact => contact.Id);

            builder.Property(contact => contact.Id).ValueGeneratedOnAdd();
            builder.Property(contact => contact.Uri).IsRequired();
            builder.Property(contact => contact.Category).HasConversion(contactCategoryConverter).IsRequired();
        }
    }
}

using karg.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using karg.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace karg.DAL.Context.EntityConfigurations
{
    internal class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            var contactCategoryConverter = new EnumToStringConverter<ContactCategory>();

            builder.ToTable("Contacts");

            builder.HasKey(contact => contact.Id);

            builder.Property(contact => contact.Id).ValueGeneratedOnAdd();
            builder.Property(contact => contact.Value).IsRequired();
            builder.Property(contact => contact.Category).HasConversion(contactCategoryConverter).IsRequired();
        }
    }
}

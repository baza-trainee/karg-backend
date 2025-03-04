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

            builder.HasData(
                new Contact { Id = 1, Category = ContactCategory.PhoneNumber, Value = "+38 (093) 986-2262" },
                new Contact { Id = 2, Category = ContactCategory.PhoneNumber, Value = "+38 (098) 844-7937" },
                new Contact { Id = 3, Category = ContactCategory.Email, Value = "karg.inform@gmail.com" },
                new Contact { Id = 4, Category = ContactCategory.LocationUa, Value = "м. Київ" },
                new Contact { Id = 5, Category = ContactCategory.LocationEn, Value = "Kyiv" },
                new Contact { Id = 6, Category = ContactCategory.Instagram, Value = "https://www.instagram.com/karg.kyiv?fbclid=IwAR1OSBKSNd-YuMMDs0Wk4yX4wnH9YZFfNU9RRpG5fhI1uQQh-cmGZV29hlg" },
                new Contact { Id = 7, Category = ContactCategory.Facebook, Value = "https://www.facebook.com/KARG.kyivanimalrescuegroup/?locale=ua_UA" },
                new Contact { Id = 8, Category = ContactCategory.Telegram, Value = "Посилання на телеграм" },
                new Contact { Id = 9, Category = ContactCategory.Statistics, Value = "2427" },
                new Contact { Id = 10, Category = ContactCategory.Statistics, Value = "2300" },
                new Contact { Id = 11, Category = ContactCategory.Statistics, Value = "720" },
                new Contact { Id = 12, Category = ContactCategory.Statistics, Value = "115" }
            );
        }
    }
}

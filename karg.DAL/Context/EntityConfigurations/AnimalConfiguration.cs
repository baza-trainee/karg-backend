using karg.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using karg.DAL.Models.Enums;

namespace karg.DAL.Context.EntityConfigurations
{
    internal class AnimalConfiguration : IEntityTypeConfiguration<Animal>
    {
        public void Configure(EntityTypeBuilder<Animal> builder)
        {
            var animalCategoryConverter = new EnumToStringConverter<AnimalCategory>();

            builder.ToTable("Animal");

            builder.HasKey(animal => animal.Id);

            builder.Property(animal => animal.Id).ValueGeneratedOnAdd().IsRequired();
            builder.Property(animal => animal.Category).HasConversion(animalCategoryConverter).IsRequired();
        }
    }
}

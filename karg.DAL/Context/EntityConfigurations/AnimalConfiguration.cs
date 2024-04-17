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

            builder.Property(animal => animal.Id).IsRequired();
            builder.Property(animal => animal.Name).HasMaxLength(30).IsRequired();
            builder.Property(animal => animal.Short_Description).HasMaxLength(50).IsRequired();
            builder.Property(animal => animal.Description).HasMaxLength(500).IsRequired();
            builder.Property(animal => animal.Story).HasMaxLength(600).IsRequired();
            builder.Property(animal => animal.Category).HasConversion(animalCategoryConverter).IsRequired();
        }
    }
}

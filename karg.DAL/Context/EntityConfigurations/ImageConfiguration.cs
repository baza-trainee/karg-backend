using karg.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace karg.DAL.Context.EntityConfigurations
{
    internal class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("Image");

            builder.HasKey(image => image.Id);

            builder.Property(image => image.Uri).IsRequired();
            builder.Property(image => image.AnimalId).IsRequired(false);
        }
    }
}

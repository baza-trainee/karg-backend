using karg.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace karg.DAL.Context.EntityConfigurations
{
    internal class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("Images");

            builder.HasKey(image => image.Id);

            builder.Property(image => image.Id).ValueGeneratedOnAdd();
            builder.Property(image => image.Uri).IsRequired();
            builder.Property(image => image.AdviceId).IsRequired(false);
            builder.Property(image => image.AnimalId).IsRequired(false);
            builder.Property(image => image.RescuerId).IsRequired(false);
            builder.Property(image => image.PartnerId).IsRequired(false);
            builder.Property(image => image.YearResultId).IsRequired(false);
        }
    }
}

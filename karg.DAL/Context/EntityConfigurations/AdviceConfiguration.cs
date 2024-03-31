using karg.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace karg.DAL.Context.EntityConfigurations
{
    internal class AdviceConfiguration : IEntityTypeConfiguration<Advice>
    {
        public void Configure(EntityTypeBuilder<Advice> builder)
        {
            builder.ToTable("Advice");

            builder.HasKey(advice => advice.Id);

            builder.Property(advice => advice.Title).HasMaxLength(100).IsRequired();
            builder.Property(advice => advice.Description).HasMaxLength(500).IsRequired();
            builder.Property(advice => advice.Created_At).IsRequired();
        }
    }
}

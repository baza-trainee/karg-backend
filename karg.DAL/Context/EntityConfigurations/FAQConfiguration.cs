using karg.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace karg.DAL.Context.EntityConfigurations
{
    internal class FAQConfiguration : IEntityTypeConfiguration<FAQ>
    {
        public void Configure(EntityTypeBuilder<FAQ> builder)
        {
            builder.ToTable("FAQ");

            builder.HasKey(faq => faq.Id);

            builder.Property(faq => faq.Answer).HasMaxLength(1500).IsRequired();
            builder.Property(faq => faq.Question).HasMaxLength(100).IsRequired();
        }
    }
}

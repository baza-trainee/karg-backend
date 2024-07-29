using karg.DAL.Context.EntityConfigurations;
using karg.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace karg.DAL.Context
{
    public class KargDbContext : DbContext
    {
        public KargDbContext(DbContextOptions<KargDbContext> options) : base(options) 
        { 
            Database.EnsureCreated();
        }
        
        public DbSet<Advice> Advices { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Rescuer> Rescuers { get; set; }
        public DbSet<YearResult> YearsResults { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<JwtToken> Tokens { get; set; }
        public DbSet<Culture> Cultures { get; set; }
        public DbSet<Localization> Localizations { get; set; }
        public DbSet<LocalizationSet> LocalizationSets { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AdviceConfiguration());
            builder.ApplyConfiguration(new AnimalConfiguration());
            builder.ApplyConfiguration(new FAQConfiguration());
            builder.ApplyConfiguration(new ImageConfiguration());
            builder.ApplyConfiguration(new RescuerConfiguration());
            builder.ApplyConfiguration(new YearResultConfiguration());
            builder.ApplyConfiguration(new PartnerConfiguration());
            builder.ApplyConfiguration(new ContactConfiguration());
            builder.ApplyConfiguration(new JwtTokenConfiguration());
            builder.ApplyConfiguration(new CultureConfiguration());
            builder.ApplyConfiguration(new LocalizationConfiguration());
            builder.ApplyConfiguration(new LocalizationSetConfiguration());

            builder.Entity<LocalizationSet>()
                .HasMany(localizationSet => localizationSet.Localizations)
                .WithOne(localization => localization.LocalizationSet)
                .HasForeignKey(localization => localization.LocalizationSetId)
                .IsRequired();

            builder.Entity<Localization>()
                .HasOne(localization => localization.Culture)
                .WithMany()
                .HasForeignKey(localization => localization.CultureCode)
                .HasPrincipalKey(culture => culture.Code)
                .IsRequired();

            builder.Entity<Rescuer>()
                .HasOne(rescuer => rescuer.Token)
                .WithOne(token => token.Rescuer)
                .HasForeignKey<Rescuer>(rescuer => rescuer.TokenId)
                .IsRequired();

            builder.Entity<Image>()
                .HasOne(image => image.Animal)
                .WithMany(animal => animal.Images)
                .HasForeignKey(image => image.AnimalId)
                .IsRequired();

            builder.Entity<Image>()
                .HasOne(image => image.Advice)
                .WithMany(advice => advice.Images)
                .HasForeignKey(image => image.AdviceId)
                .IsRequired();

            builder.Entity<Image>()
                .HasOne(image => image.Rescuer)
                .WithMany(rescuer => rescuer.Images)
                .HasForeignKey(image => image.RescuerId)
                .IsRequired();

            builder.Entity<Image>()
                .HasOne(image => image.Partner)
                .WithMany(partner => partner.Images)
                .HasForeignKey(image => image.PartnerId)
                .IsRequired();

            builder.Entity<Image>()
                .HasOne(image => image.YearResult)
                .WithMany(yearResult => yearResult.Images)
                .HasForeignKey(image => image.YearResultId)
                .IsRequired();

            base.OnModelCreating(builder);
        }
    }
}

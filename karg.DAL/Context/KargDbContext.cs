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
        public DbSet<YearResult> YearResults { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<JwtToken> Tokens { get; set; }
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

            builder.Entity<Advice>()
                .HasOne(advice => advice.Image)
                .WithOne(image => image.Advice)
                .HasForeignKey<Advice>(advice => advice.ImageId)
                .IsRequired();

            builder.Entity<Rescuer>()
                .HasOne(rescuer => rescuer.Token)
                .WithOne(token => token.Rescuer)
                .HasForeignKey<Rescuer>(rescuer => rescuer.TokenId)
                .IsRequired();

            builder.Entity<Rescuer>()
                .HasOne(rescuer => rescuer.Image)
                .WithOne(image => image.Rescuer)
                .HasForeignKey<Rescuer>(rescuer => rescuer.ImageId)
                .IsRequired();

            builder.Entity<Partner>()
                .HasOne(partner => partner.Image)
                .WithOne(image => image.Partner)
                .HasForeignKey<Partner>(partner => partner.ImageId)
                .IsRequired();

            builder.Entity<YearResult>()
                .HasOne(yearResult => yearResult.Image)
                .WithOne(image => image.YearResult)
                .HasForeignKey<YearResult>(yearResult => yearResult.ImageId)
                .IsRequired();

            builder.Entity<Image>()
                .HasOne(image => image.Animal)
                .WithMany(animal => animal.Images)
                .HasForeignKey(image => image.AnimalId)
                .IsRequired();

            base.OnModelCreating(builder);
        }
    }
}

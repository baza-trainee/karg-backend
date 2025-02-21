using karg.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using karg.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace karg.DAL.Context.EntityConfigurations
{
    internal class RescuerConfiguration : IEntityTypeConfiguration<Rescuer>
    {
        public void Configure(EntityTypeBuilder<Rescuer> builder)
        {
            var rescuerRoleConverter = new EnumToStringConverter<RescuerRole>();

            builder.ToTable("Rescuers");

            builder.HasKey(rescuer => rescuer.Id);

            builder.Property(rescuer => rescuer.Id).ValueGeneratedOnAdd();
            builder.Property(rescuer => rescuer.FullName).HasMaxLength(50).IsRequired();
            builder.Property(rescuer => rescuer.Email).HasMaxLength(50).IsRequired();
            builder.Property(rescuer => rescuer.PhoneNumber).HasMaxLength(50);
            builder.Property(rescuer => rescuer.Current_Password).HasMaxLength(64).IsRequired();
            builder.Property(rescuer => rescuer.Role).HasConversion(rescuerRoleConverter).IsRequired();


            builder.HasData(
                new Rescuer { Id = 1, Email = "admin@gmail.com", Current_Password = "001He87I8P1n8k7a70SJizxEyQdPQsTGcSOgRls0V8Y=", FullName = "Admin KARG", Role = RescuerRole.Director, TokenId = 1 }
            );
        }
    }
}

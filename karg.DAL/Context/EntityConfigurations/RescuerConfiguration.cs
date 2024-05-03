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

            builder.ToTable("Rescuer");

            builder.HasKey(rescuer => rescuer.Id);

            builder.Property(rescuer => rescuer.Id).ValueGeneratedOnAdd();
            builder.Property(rescuer => rescuer.FullName).HasMaxLength(50).IsRequired();
            builder.Property(rescuer => rescuer.Email).HasMaxLength(50).IsRequired();
            builder.Property(rescuer => rescuer.PhoneNumber).HasMaxLength(50);
            builder.Property(rescuer => rescuer.Current_Password).HasMaxLength(64).IsRequired();
            builder.Property(animal => animal.Role).HasConversion(rescuerRoleConverter).IsRequired();
        }
    }
}

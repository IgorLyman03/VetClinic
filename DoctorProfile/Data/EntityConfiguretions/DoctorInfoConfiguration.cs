using DoctorProfile.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoctorProfile.Data.EntityConfiguretions
{
    public class DoctorInfoConfiguration : IEntityTypeConfiguration<DoctorInfo>
    {
        public void Configure(EntityTypeBuilder<DoctorInfo> builder)
        {
            builder.HasKey(di => di.Id);

            builder.Property(di => di.Id)
                .ValueGeneratedOnAdd();

            builder.Property(di => di.FullName)
                   .IsRequired();

            builder.Property(di => di.Email)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.HasIndex(di => di.Email)
                .IsUnique();

            builder.Property(di => di.UserId)
               .IsRequired()
               .HasMaxLength(255);

            builder.HasIndex(di => di.UserId)
                .IsUnique();

            builder.Property(di => di.Specialization)
                   .IsRequired();
        }
    }
}

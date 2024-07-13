using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DoctorProfile.Data.Entities;

namespace DoctorProfile.Data.EntityConfiguretions
{

    public class DoctorTimetableConfiguration : IEntityTypeConfiguration<DoctorTimetable>
    {
        public void Configure(EntityTypeBuilder<DoctorTimetable> builder)
        {
            builder.HasKey(dt => dt.Id);

            builder.Property(dt => dt.Id)
                .ValueGeneratedOnAdd();

            builder.Property(dt => dt.UserId)
               .IsRequired()
               .HasMaxLength(255);

            builder.HasOne(dt => dt.DoctorInfo)
                .WithMany(dp => dp.DoctorTimetables)
                .HasForeignKey(dt => dt.UserId)
                .HasPrincipalKey(dp => dp.UserId);

            builder.Property(dt => dt.StartTime)
                   .IsRequired();

            builder.Property(dt => dt.EndTime)
                   .IsRequired();
        }
    }
}

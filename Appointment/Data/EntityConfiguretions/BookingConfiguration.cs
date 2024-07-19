using Appointment.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointment.Data.EntityConfiguretions
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.StartDate)
                   .IsRequired();

            builder.Property(b => b.EndDate)
                .IsRequired();

            builder.Property(b => b.BookingNode)
               .HasMaxLength(1000);

            builder.Property(b => b.Status)
                .IsRequired();
        }
    }
}

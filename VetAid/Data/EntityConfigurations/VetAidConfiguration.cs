using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using VetAid.Data.Entities;

namespace VetAid.Data.EntityConfigurations
{
    public class VetAidConfiguration : IEntityTypeConfiguration<VetAidEntity>
    {
        public void Configure(EntityTypeBuilder<VetAidEntity> builder)
        {
            builder.ToTable("VetAids"); 
            builder.HasKey(v => v.Id); 
            builder.Property(v => v.Name).HasMaxLength(100);
            builder.Property(v => v.Description).HasMaxLength(500);
            builder.Property(v => v.ServiceType).HasMaxLength(50);
            builder.HasMany(v => v.AnimalTypes)
                .WithMany(a => a.vetAids).UsingEntity<Dictionary<string, object>>(
                    "VetAidAnimalType",
                    j => j
                        .HasOne<AnimalTypeEntity>()
                        .WithMany()
                        .HasForeignKey("VetAidId"),
                    j => j
                        .HasOne<VetAidEntity>()
                        .WithMany()
                        .HasForeignKey("AnimalTypeId"),
                    j =>
                    {
                        j.HasKey("VetAidId", "AnimalTypeId");
                        j.ToTable("VetAidAnimalType");
                    });
        }
    }
}

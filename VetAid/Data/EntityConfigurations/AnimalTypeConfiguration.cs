

namespace VetAid.Data.EntityConfigurations
{
    public class AnimalTypeConfiguration: IEntityTypeConfiguration<AnimalTypeEntity>
    {
        public void Configure(EntityTypeBuilder<AnimalTypeEntity> builder)
        {
            builder.ToTable("AnimalTypes");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name).HasMaxLength(100); 

        }
    }
}

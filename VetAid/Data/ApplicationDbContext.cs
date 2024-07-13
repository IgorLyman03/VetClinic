namespace VetAid.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<AnimalTypeEntity> AnimalTypes { get; set; } = null!;
        public DbSet<VetAidEntity> VetAids { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AnimalTypeConfiguration());
            builder.ApplyConfiguration(new VetAidConfiguration());
        }
    }
}

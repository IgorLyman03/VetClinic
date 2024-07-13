namespace VetAid.Data.Entities
{
    public class VetAidEntity
    {
        public int? Id { get; set; }
        public string? Name { get; set; } = null;
        public string? Description { get; set; } = null;
        public string? ServiceType { get; set; } = null; 
        public IEnumerable<AnimalTypeEntity>? AnimalTypes { get; set; }
        public TimeSpan? Duration {  get; set; } = TimeSpan.FromMinutes(60);
        public decimal? Price { get; set; }
    }
}

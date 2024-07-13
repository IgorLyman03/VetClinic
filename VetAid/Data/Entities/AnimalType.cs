namespace VetAid.Data.Entities
{
    public class AnimalTypeEntity
    {
        public int? Id { get; set; }
        public string? Name { get; set; } = null;
        public IEnumerable<VetAidEntity>? vetAids { get; set; }
    }
}

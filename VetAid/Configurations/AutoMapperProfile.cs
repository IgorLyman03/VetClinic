namespace VetAid.Configurations
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AnimalTypeEntity, AnimalTypeDto>().ReverseMap();
            CreateMap<VetAidEntity, VetAidDto>()
                .ForMember(dto => dto.AnimalTypes, opt => opt.MapFrom(e => e.AnimalTypes.Select(t => t))).
                ReverseMap()
                .ForMember(e => e.AnimalTypes, opt => opt.MapFrom(dto => dto.AnimalTypes.Select(t => new AnimalTypeEntity() { Id = t.Id })));
        }
    }
}

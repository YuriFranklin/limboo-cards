namespace LimbooCards.Application.Mappings
{
    using AutoMapper;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Application.DTOs;

    public class PlannerMappingProfile : Profile
    {
        public PlannerMappingProfile()
        {
            CreateMap<Planner, PlannerDto>().ReverseMap();
            CreateMap<CreatePlannerDto, Planner>()
            .ForCtorParam("id", opt => opt.MapFrom(src => src.Id));

            CreateMap<PlannerBucket, PlannerBucketDto>().ReverseMap();
            CreateMap<PinRule, PinRuleDto>().ReverseMap();
        }
    }
}
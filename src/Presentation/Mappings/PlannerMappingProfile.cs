namespace LimbooCards.Presentation.Mappings
{
    using AutoMapper;
    using LimbooCards.Application.DTOs;
    using LimbooCards.Presentation.GraphQL.Contracts;
    using LimbooCards.Presentation.GraphQL.Models;
    public class PlannerMappingProfile : Profile
    {
        public PlannerMappingProfile()
        {
            CreateMap<PlannerDto, PlannerModel>().ReverseMap();
            CreateMap<PlannerBucketModel, PlannerBucketDto>().ReverseMap();
            CreateMap<PinRuleModel, PinRuleDto>().ReverseMap();
            CreateMap<CreatePlannerInput, CreatePlannerDto>().ReverseMap();
        }
    }
}
namespace LimbooCards.Infra.Mappings
{
    using AutoMapper;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Infra.DTOs;
    public class SubjectPublisherMappingProfile : Profile
    {
        public SubjectPublisherMappingProfile()
        {
            CreateMap<SubjectPublisherAutomateDto, SubjectPublisher>()
            .ForCtorParam("name", opt => opt.MapFrom(dto => dto.NOME ?? string.Empty))
            .ForCtorParam("IsCurrent", opt => opt.MapFrom(dto => dto.IS_CURRENT ?? null))
            .ForCtorParam("IsExpect", opt => opt.MapFrom(dto => dto.IS_EXPECTED ?? null));
        }
    }
}
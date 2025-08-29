namespace LimbooCards.Infra.Mappings
{
    using AutoMapper;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Infra.DTOs;

    public class CardMappingProfile : Profile
    {
        public CardMappingProfile()
        {
            CreateMap<CardAutomateDto, Card>()
            .ForCtorParam("createdBy", opt => opt.MapFrom(src => src.createdBy.user.id))
            .ForCtorParam("createdAt", opt => opt.MapFrom(src => DateTime.Parse(src.createdDateTime)))
            .ForCtorParam("dueDateTime", opt => opt.MapFrom(src => DateTime.Parse(src.dueDateTime ?? string.Empty)));
        }
    }
}
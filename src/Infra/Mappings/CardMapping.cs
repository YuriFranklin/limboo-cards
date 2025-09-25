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
            .ForCtorParam("createdAt", opt => opt.MapFrom(src =>
                string.IsNullOrWhiteSpace(src.createdDateTime) ? (DateTime?)null : DateTime.Parse(src.createdDateTime)))
            .ForCtorParam("dueDateTime", opt => opt.MapFrom(src =>
                string.IsNullOrWhiteSpace(src.dueDateTime) ? (DateTime?)null : DateTime.Parse(src.dueDateTime)))
            .ForCtorParam("subjectId", opt => opt.MapFrom(src =>
                string.IsNullOrWhiteSpace(src.subjectId) ? (Guid?)null : Guid.Parse(src.subjectId)));
        }
    }
}
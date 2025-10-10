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
                .ForCtorParam("createdBy", opt => opt.MapFrom(src => src.CreatedBy.User.Id))
                .ForCtorParam("createdAt", opt => opt.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.CreatedDateTime) ? (DateTime?)null : DateTime.Parse(src.CreatedDateTime)))
                .ForCtorParam("dueDateTime", opt => opt.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.DueDateTime) ? (DateTime?)null : DateTime.Parse(src.DueDateTime)))
                .ForCtorParam("subjectId", opt => opt.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.SubjectId) ? (Guid?)null : Guid.Parse(src.SubjectId)));

            CreateMap<Card, CardAutomateDto>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src =>
                    new CreatedBy { User = new CreatedByUser { Id = src.CreatedBy } }))

                .ForMember(dest => dest.CreatedDateTime, opt => opt.MapFrom(src =>
                    src.CreatedAt.ToString("o")))

                .ForMember(dest => dest.DueDateTime, opt => opt.MapFrom(src =>
                    src.DueDateTime.HasValue ? src.DueDateTime.Value.ToString("o") : null))

                .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src =>
                    src.SubjectId.HasValue ? src.SubjectId.Value.ToString() : string.Empty));
        }
    }
}
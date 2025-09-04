namespace LimbooCards.Infra.Mappings
{
    using System.Text.Json;
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
            .ForCtorParam("dueDateTime", opt => opt.MapFrom(src => DateTime.Parse(src.dueDateTime ?? string.Empty)))
            .ForCtorParam("subjectId", opt => opt.MapFrom(dto =>
                ExtractSubjectId(dto.description)
            ));
        }

        private static Guid? ExtractSubjectId(string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return null;

            try
            {
                using var doc = JsonDocument.Parse(description);
                if (doc.RootElement.TryGetProperty("subjectId", out var subjectIdElement))
                {
                    var value = subjectIdElement.GetString();
                    return Guid.TryParse(value, out var guid) ? guid : null;
                }
            }
            catch
            {
                return null;
            }

            return null;
        }
    }
}
namespace LimbooCards.Infra.Mappings
{
    using AutoMapper;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Infra.DTOs;

    public class SubjectMappingProfile : Profile
    {
        public SubjectMappingProfile()
        {

            CreateMap<SubjectAutomateDto, Subject>()
            .ForCtorParam("id", opt => opt.MapFrom(dto =>
                string.IsNullOrWhiteSpace(dto.UUID) ? (Guid?)null : Guid.Parse(dto.UUID)
            ))
            .ForCtorParam("name", opt => opt.MapFrom(dto => dto.DISCIPLINA ?? string.Empty))
            .ForCtorParam("semester", opt => opt.MapFrom(_ => "20252"))
            .ForCtorParam("status", opt => opt.MapFrom(dto => SubjectStatusMapper.FromAutomateString(dto.STATUS_DIG) ?? null))
            .ForCtorParam("oferts", opt => opt.MapFrom(dto => OfertMapper.FromAutomateString(dto.OFERTAS)))
            .ForCtorParam("equivalencies", opt => opt.MapFrom(dto => EquivalenciesMapper.FromAutomateString(dto.EQUIVALENCIA)))
            .ForCtorParam("contents", opt => opt.MapFrom(dto =>
                ContentMapper.FromAutomateStrings(new Dictionary<string, string?> {
                    { "BANNER", dto.BANNER },
                    { "DC", dto.DC },
                    { "AC", dto.AC },
                    { "VIDEOTECA_1", dto.VIDEOTECA_1 },
                    { "VIDEOTECA_2", dto.VIDEOTECA_2 },
                    { "VIDEOTECA_3", dto.VIDEOTECA_3 },
                    { "VIDEOTECA_4", dto.VIDEOTECA_4 },
                    { "E_BOOK", dto.E_BOOK },
                    { "MATERIAL_1", dto.MATERIAL_1 },
                    { "MATERIAL_2", dto.MATERIAL_2 },
                    { "MATERIAL_3", dto.MATERIAL_3 },
                    { "MATERIAL_4", dto.MATERIAL_4 },
                    { "BQ_1", dto.BQ_1 },
                    { "BQ_2", dto.BQ_2 },
                    { "BQ_3", dto.BQ_3 },
                    { "BQ_4", dto.BQ_4 }
                    })
            ))
            .ForCtorParam("owner", opt => opt.MapFrom(dto => GetOwner(dto.OWNERS)))
            .ForCtorParam("coOwners", opt => opt.MapFrom(dto => GetCoOwners(dto.OWNERS)))
            .ForCtorParam("publishers", opt => opt.MapFrom(dto => dto.PUBLISHERS));
        }

        private static List<UserAutomateDto>? GetCoOwners(List<UserAutomateDto>? owners)
        {
            return owners != null && owners.Count > 1 ? [.. owners.Skip(1)] : new List<UserAutomateDto>();
        }

        private static UserAutomateDto? GetOwner(List<UserAutomateDto>? owners)
        {
            return owners != null && owners.Count >= 1 ? owners[0] : null;
        }
    }
}

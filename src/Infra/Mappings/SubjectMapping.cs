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
            .ForCtorParam("modelId", opt => opt.MapFrom(dto =>
                string.IsNullOrWhiteSpace(dto.ID) ? null : dto.ID
            ))
            .ForCtorParam("name", opt => opt.MapFrom(dto => dto.DISCIPLINA ?? string.Empty))
            .ForCtorParam("semester", opt => opt.MapFrom(_ => "20252"))
            .ForCtorParam("status", opt => opt.MapFrom(dto => SubjectStatusMapper.FromAutomateString(dto.STATUS_DIG) ?? null))
            .ForCtorParam("oferts", opt => opt.MapFrom(dto => OfertMapper.FromAutomateString(dto.OFERTAS)))
            .ForCtorParam("equivalencies", opt => opt.MapFrom(dto => EquivalenciesMapper.FromAutomateString(dto.EQUIVALENCIA)))
            .ForCtorParam("contents", opt => opt.MapFrom(dto => BuildContentsDictionary(dto)))
            .ForCtorParam("owner", opt => opt.MapFrom(dto => GetOwner(dto.OWNERS)))
            .ForCtorParam("coOwners", opt => opt.MapFrom(dto => GetCoOwners(dto.OWNERS)))
            .ForCtorParam("publishers", opt => opt.MapFrom(dto =>
                dto.PUBLISHERS != null
                    ? dto.PUBLISHERS.Where(p => !string.IsNullOrWhiteSpace(p.NOME))
                    : new List<SubjectPublisherAutomateDto>()
            ));
        }

        private static List<UserAutomateDto>? GetCoOwners(List<UserAutomateDto>? owners)
        {
            return owners != null && owners.Count > 1 ? [.. owners.Skip(1)] : new List<UserAutomateDto>();
        }

        private static UserAutomateDto? GetOwner(List<UserAutomateDto>? owners)
        {
            return owners != null && owners.Count >= 1 ? owners[0] : null;
        }

        private static Dictionary<string, string?> BuildContentsDictionary(SubjectAutomateDto dto)
        {
            var pairs = new[]
            {
                new KeyValuePair<string,string?>("BANNER", dto.BANNER),
                new KeyValuePair<string,string?>("DC", dto.DC),
                new KeyValuePair<string,string?>("AC", dto.AC),
                new KeyValuePair<string,string?>("VIDEOTECA_1", dto.VIDEOTECA_1),
                new KeyValuePair<string,string?>("VIDEOTECA_2", dto.VIDEOTECA_2),
                new KeyValuePair<string,string?>("VIDEOTECA_3", dto.VIDEOTECA_3),
                new KeyValuePair<string,string?>("VIDEOTECA_4", dto.VIDEOTECA_4),
                new KeyValuePair<string,string?>("E_BOOK", dto.E_BOOK),
                new KeyValuePair<string,string?>("MATERIAL_1", dto.MATERIAL_1),
                new KeyValuePair<string,string?>("MATERIAL_2", dto.MATERIAL_2),
                new KeyValuePair<string,string?>("MATERIAL_3", dto.MATERIAL_3),
                new KeyValuePair<string,string?>("MATERIAL_4", dto.MATERIAL_4),
                new KeyValuePair<string,string?>("BQ_1", dto.BQ_1),
                new KeyValuePair<string,string?>("BQ_2", dto.BQ_2),
                new KeyValuePair<string,string?>("BQ_3", dto.BQ_3),
                new KeyValuePair<string,string?>("BQ_4", dto.BQ_4),
            };

            return pairs
                .Where(kvp => !string.IsNullOrWhiteSpace(kvp.Value))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}

namespace LimbooCards.Infra.Mappings
{
    using AutoMapper;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Shared;
    using LimbooCards.Infra.DTOs;
    using System.Reflection;
    using System.Text;

    public class SubjectMappingProfile : Profile
    {
        public SubjectMappingProfile()
        {
            CreateMap<SubjectAutomateDto, Subject>()
                .ForCtorParam("id", opt => opt.MapFrom(dto => string.IsNullOrWhiteSpace(dto.Uuid) ? (Guid?)null : Guid.Parse(dto.Uuid)))
                .ForCtorParam("modelId", opt => opt.MapFrom(dto => string.IsNullOrWhiteSpace(dto.Id) ? null : dto.Id))
                .ForCtorParam("name", opt => opt.MapFrom(dto => dto.Disciplina ?? string.Empty))
                .ForCtorParam("semester", opt => opt.MapFrom(_ => "20252"))
                .ForCtorParam("status", opt => opt.MapFrom(dto => SubjectStatusMapper.FromAutomateString(dto.Status) ?? null))
                .ForCtorParam("oferts", opt => opt.MapFrom(dto => OfertMapper.FromAutomateString(dto.Ofertas)))
                .ForCtorParam("equivalencies", opt => opt.MapFrom(dto => EquivalenciesMapper.FromAutomateString(dto.Equivalencia)))
                .ForCtorParam("owner", opt => opt.MapFrom(dto => GetOwner(dto.Owners)))
                .ForCtorParam("coOwners", opt => opt.MapFrom(dto => GetCoOwners(dto.Owners)))
                .ForCtorParam("publishers", opt => opt.MapFrom(dto => dto.Publishers != null ? dto.Publishers.Where(p => !string.IsNullOrWhiteSpace(p.NOME)).ToList() : new List<SubjectPublisherAutomateDto>()))
                .ForCtorParam("isPractical", opt => opt.MapFrom(dto => (dto.Pratica ?? "NÃO").Equals("SIM", StringComparison.OrdinalIgnoreCase)))
                .ForCtorParam("contents", opt => opt.MapFrom(dto => BuildContentsDictionary(dto)));
        }

        private static List<UserAutomateDto>? GetCoOwners(List<UserAutomateDto>? owners)
        {
            return owners?.Skip(1).ToList();
        }

        private static UserAutomateDto? GetOwner(List<UserAutomateDto>? owners)
        {
            return owners?.FirstOrDefault();
        }

        private static Dictionary<string, string?> BuildContentsDictionary(SubjectAutomateDto dto)
        {
            var contents = new Dictionary<string, string?>();
            var dtoType = typeof(SubjectAutomateDto);

            foreach (var contentKey in ContentTitles.Map.Keys)
            {
                var propertyName = ConvertKeyToPropertyName(contentKey);
                PropertyInfo? propInfo = dtoType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                if (propInfo != null)
                {
                    var value = propInfo.GetValue(dto) as string;

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        contents[contentKey] = value;
                    }
                }
            }
            return contents;
        }

        private static string ConvertKeyToPropertyName(string key)
        {
            var parts = key.Split('_');
            var sb = new StringBuilder();
            foreach (var part in parts)
            {
                if (string.IsNullOrEmpty(part)) continue;
                sb.Append(char.ToUpper(part[0]));
                sb.Append(part[1..].ToLower());
            }
            return sb.ToString();
        }
    }
}
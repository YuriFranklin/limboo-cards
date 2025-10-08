namespace LimbooCards.Infra.Mappings
{
    using AutoMapper;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Shared;

    public class ContentMappingProfile : Profile
    {
        public ContentMappingProfile()
        {
            CreateMap<KeyValuePair<string, string?>, Content>()
                .ForCtorParam("name", opt => opt.MapFrom(src => src.Key))
                .ForCtorParam("checklistItemTitle", opt => opt.MapFrom(src => FormatTitle(src.Key)))
                .ForCtorParam("status", opt => opt.MapFrom(src =>
                    ContentStatusMapper.FromAutomateString(src.Value)
                ));
        }

        private static string FormatTitle(string key)
        {
            var title = ContentTitles.Map.TryGetValue(key, out var niceTitle)
                ? niceTitle
                : key;

            return $"[{key}] {title}";
        }
    }
}

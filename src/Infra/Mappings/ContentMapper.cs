namespace LimbooCards.Infra.Mappings
{
    using LimbooCards.Domain.Entities;
    using LimbooCards.Infra.Constants;

    public static class ContentMapper
    {
        public static List<Content>? FromAutomateStrings(Dictionary<string, string?>? rawContents)
        {
            if (rawContents == null || rawContents.Count == 0)
                return null;

            var contents = rawContents
                .Where(rc => !string.IsNullOrWhiteSpace(rc.Value))
                .Select(rc =>
                {
                    var title = ContentTitles.Map.TryGetValue(rc.Key, out var niceTitle)
                        ? niceTitle
                        : rc.Key;

                    var finalTitle = $"[{rc.Key}] {title}";

                    return new Content(
                        name: rc.Key!,
                        checklistItemTitle: finalTitle,
                        status: ContentStatusMapper.FromAutomateString(rc.Value)
                    );
                })
                .ToList();

            return contents.Count > 0 ? contents : null;
        }
    }
}

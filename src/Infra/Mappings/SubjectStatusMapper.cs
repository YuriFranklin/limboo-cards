namespace LimbooCards.Infra.Mappings
{
    using LimbooCards.Domain.Shared;

    public static class SubjectStatusMapper
    {
        public static SubjectStatus? FromAutomateString(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return null;

            return raw.ToUpperInvariant() switch
            {
                "OK" => SubjectStatus.Complete,
                "INCOMPLETE" => SubjectStatus.Incomplete,
                "MISSING" => SubjectStatus.Missing,
                _ => null
            };
        }
    }
}
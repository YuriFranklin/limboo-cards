namespace LimbooCards.Infra.Mappings
{
    using LimbooCards.Domain.Entities;

    public static class ContentStatusMapper
    {
        public static ContentStatus? FromAutomateString(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw) || raw.Trim().ToUpperInvariant() == "NÃO SE APLICA")
                return null;

            return raw.Trim().ToUpperInvariant() switch
            {
                "OK" => ContentStatus.OK,
                "FALTA" => ContentStatus.Missing,
                _ => null
            };
        }
    }
}

namespace LimbooCards.Infra.Mappings
{
    public static class EquivalenciesMapper
    {
        public static List<string>? FromAutomateString(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return null;

            var parts = raw.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var result = new List<string>();
            result.AddRange(parts);
            return result;
        }
    }
}
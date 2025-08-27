namespace LimbooCards.Infra.Mappings
{
    using LimbooCards.Domain.Entities;

    public static class OfertMapper
    {
        public static List<Ofert> FromAutomateString(string? raw)
        {
            var oferts = new List<Ofert>();

            if (string.IsNullOrWhiteSpace(raw))
                return oferts;

            var parts = raw.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var part in parts)
            {
                var trimmed = part.Trim().TrimStart('[').TrimEnd(']');

                var segments = trimmed.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (segments.Length == 2)
                {
                    var project = segments[0].Trim();
                    var module = segments[1].Trim();

                    if (module.Equals("NULL", StringComparison.OrdinalIgnoreCase))
                        module = string.Empty;

                    oferts.Add(new Ofert(project, module));
                }
            }

            return oferts;
        }

    }
}
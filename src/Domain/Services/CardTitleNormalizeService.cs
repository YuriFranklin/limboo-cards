namespace LimbooCards.Domain.Services
{
    using System.Text.RegularExpressions;
    using LimbooCards.Domain.Entities;
    using System.Globalization;

    public static partial class CardTitleNormalizeService
    {
        // Regex única e poderosa para fazer o "parse" de todos os formatos
        [GeneratedRegex(
            @"^ (?: \[?\s*PEND[EÊ]NCIAS?\s*(?:-\s*(?<modelId>[^\]\s]*))?\s*\]? )? \s* (?: \[\s*(?<modelId>\d+)\s*\] )? \s* (?<name>.+) $",
            RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace)]
        private static partial Regex UnnormalizePattern();

        public static string Normalize(Subject subject)
        {
            ArgumentNullException.ThrowIfNull(subject);
            if (string.IsNullOrWhiteSpace(subject.ModelId)) throw new ArgumentException("Subject.ModelId cannot be null or empty.", nameof(subject));
            if (string.IsNullOrWhiteSpace(subject.Name)) throw new ArgumentException("Subject.Name cannot be null or empty.", nameof(subject));

            return $"[PENDÊNCIA - {subject.ModelId}] {subject.Name.ToUpper()}";
        }

        public static (string ModelId, string Name)? Unnormalize(string normalized)
        {
            if (string.IsNullOrWhiteSpace(normalized))
                return null;

            var match = UnnormalizePattern().Match(normalized);

            if (!match.Success)
            {
                return (string.Empty, normalized);
            }

            var modelId = match.Groups["modelId"].Captures
                               .Cast<Capture>()
                               .LastOrDefault(c => !string.IsNullOrEmpty(c.Value))?.Value ?? string.Empty;

            var rawName = match.Groups["name"].Value.Trim();

            var textInfo = new CultureInfo("pt-BR").TextInfo;
            var titleCaseName = textInfo.ToTitleCase(rawName.ToLower());

            return (modelId.Trim(), titleCaseName);
        }
    }
}
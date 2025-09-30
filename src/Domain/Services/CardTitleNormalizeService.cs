namespace LimbooCards.Domain.Services
{
    using System.Text.RegularExpressions;
    using LimbooCards.Domain.Entities;
    using System.Globalization;

    public static class CardTitleNormalizeService
    {
        private static readonly Regex _pattern = new(
            @"^\[PENDÊNCIA - (?<modelId>[^\]]+)\]\s(?<name>.+)$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

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

            var match = _pattern.Match(normalized);
            if (!match.Success)
                return null;

            var modelId = match.Groups["modelId"].Value.Trim();
            var rawName = match.Groups["name"].Value.Trim();

            var textInfo = new CultureInfo("pt-BR").TextInfo;
            var titleCaseName = textInfo.ToTitleCase(rawName.ToLower());

            return (modelId, titleCaseName);
        }
    }
}

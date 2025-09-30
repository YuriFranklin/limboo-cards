using LimbooCards.Domain.Entities;
using System.Globalization;
using System.Text;

namespace LimbooCards.Domain.Services
{
    public class CardSubjectMatcherService(ISynonymProvider synonymProvider)
    {
        private const double MatchThreshold = 0.5;
        private readonly ISynonymProvider _synonymProvider = synonymProvider;

        public async Task<Subject?> MatchSubjectForCardAsync(Card card, IEnumerable<Subject> subjects)
        {
            if (card.SubjectId.HasValue)
            {
                var existing = subjects.FirstOrDefault(s => s.Id == card.SubjectId.Value);
                if (existing != null)
                {
                    return existing;
                }

            }

            var directMatch = subjects.FirstOrDefault(s =>
            {
                var un = CardTitleNormalizeService.Unnormalize(card.Title);
                return un is { } u &&
                    string.Equals(u.Name, s.Name, StringComparison.OrdinalIgnoreCase);
            });

            if (directMatch != null)
                return directMatch;

            var cardTokens = await NormalizeTokensAsync(card.Title);
            if (cardTokens.Count == 0)
                return null;

            Subject? bestMatch = null;
            double bestScore = 0;

            foreach (var subject in subjects)
            {
                var subjectTokens = await NormalizeTokensAsync(subject.Name);
                double score = CalculateJaccardSimilarity(cardTokens, subjectTokens);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMatch = subject;
                }
            }

            return bestScore >= MatchThreshold ? bestMatch : null;
        }

        private static double CalculateJaccardSimilarity(IEnumerable<string> a, IEnumerable<string> b)
        {
            var setA = new HashSet<string>(a, StringComparer.OrdinalIgnoreCase);
            var setB = new HashSet<string>(b, StringComparer.OrdinalIgnoreCase);

            if (setA.Count == 0 && setB.Count == 0) return 1.0;
            if (setA.Count == 0 || setB.Count == 0) return 0.0;

            return (double)setA.Intersect(setB, StringComparer.OrdinalIgnoreCase).Count()
                 / setA.Union(setB, StringComparer.OrdinalIgnoreCase).Count();
        }

        private static readonly HashSet<string> StopWords = new(StringComparer.OrdinalIgnoreCase)
        {
            "da","de","do","das","dos","a","o","e","para","por","na","no","em"
        };

        private static readonly Dictionary<string, string> PortugueseNumberWords = new(StringComparer.OrdinalIgnoreCase)
        {
            { "um", "1" }, { "uma", "1" }, { "dois", "2" }, { "duas", "2" },
            { "tres", "3" }, { "três", "3" },
            { "quatro", "4" }, { "cinco", "5" }, { "seis", "6" },
            { "sete", "7" }, { "oito", "8" }, { "nove", "9" }, { "dez", "10" }
        };

        private static int RomanToInt(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0;

            s = s.ToUpperInvariant();
            foreach (var c in s)
                if ("MDCLXVI".IndexOf(c) < 0)
                    return 0;

            var map = new Dictionary<char, int>
            {
                { 'M', 1000 }, { 'D', 500 }, { 'C', 100 },
                { 'L', 50 }, { 'X', 10 }, { 'V', 5 }, { 'I', 1 }
            };

            int total = 0, prev = 0;
            for (int i = s.Length - 1; i >= 0; i--)
            {
                int val = map[s[i]];
                if (val < prev) total -= val;
                else { total += val; prev = val; }
            }
            return total;
        }

        private async Task<List<string>> NormalizeTokensAsync(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new List<string>();

            // Remove acentos
            string normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var ch in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }
            string noDiacritics = sb.ToString().Normalize(NormalizationForm.FormC);

            sb.Clear();
            foreach (char c in noDiacritics)
                sb.Append(char.IsLetterOrDigit(c) ? char.ToLowerInvariant(c) : ' ');

            var rawTokens = sb.ToString()
                              .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                              .Select(t => t.Trim())
                              .ToList();

            var outTokens = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var token in rawTokens)
            {
                if (StopWords.Contains(token))
                    continue;

                int roman = RomanToInt(token);
                if (roman > 0) { outTokens.Add(roman.ToString()); continue; }

                if (PortugueseNumberWords.TryGetValue(token, out var numWord))
                { outTokens.Add(numWord); continue; }

                outTokens.Add(token);

                var synonyms = await _synonymProvider.GetSynonymsAsync(token);
                foreach (var syn in synonyms)
                    outTokens.Add(syn);
            }

            return [.. outTokens];
        }
    }
}

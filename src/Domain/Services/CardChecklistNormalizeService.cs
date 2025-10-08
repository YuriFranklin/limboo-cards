using System.Globalization;
using System.Text;
using LimbooCards.Domain.Entities;
using LimbooCards.Domain.Events;
using LimbooCards.Domain.Shared;

namespace LimbooCards.Domain.Services
{
    public class CardChecklistNormalizeService
    {
        private const double WeightTokenJaccard = 0.5;
        private const double WeightJaro = 0.3;
        private const double WeightLevenshtein = 0.2;
        private const double MatchThreshold = 0.5;

        public static List<ChecklistItemNormalized> NormalizeChecklist(Card card)
        {
            var normalizedItems = new List<ChecklistItemNormalized>();
            if (card.Checklist == null || card.Checklist.Count == 0) return normalizedItems;

            var contentTitles = ContentTitles.Map;

            foreach (var item in card.Checklist)
            {
                var itemTokens = NormalizeTokens(item.Title);
                (string Key, string Value, double Score) bestMatch = (string.Empty, string.Empty, 0.0);

                foreach (var content in contentTitles)
                {
                    var contentTokens = NormalizeTokens(content.Value);

                    double jaccard = TokenJaccard(itemTokens, contentTokens);
                    double jaro = JaroWinkler(item.Title, content.Value);
                    double levenshtein = LevenshteinSimilarity(item.Title, content.Value);

                    double score = (jaccard * WeightTokenJaccard) +
                                (jaro * WeightJaro) +
                                (levenshtein * WeightLevenshtein);

                    if (score > bestMatch.Score)
                        bestMatch = (content.Key, content.Value, score);
                }

                if (bestMatch.Score < MatchThreshold)
                {
                    /* throw new InvalidOperationException(
                        $"No reliable match found for '{item.Title}'. " +
                        $"Best score: {bestMatch.Score:F2}"); */
                    continue;
                }

                normalizedItems.Add(
                    new ChecklistItemNormalized(card.Id!, item.Id,
                        FormatResult(bestMatch.Key, bestMatch.Value)));
            }
            return normalizedItems;
        }

        private static readonly Dictionary<string, string> Synonyms = new(StringComparer.OrdinalIgnoreCase)
        {
            { "und", "unidade" }, { "un", "unidade" }, { "uni", "unidade" }, { "u", "unidade" },
            { "video", "videoteca" }, { "videoteca", "videoteca" }, { "vid", "videoteca" },
            { "material", "material" }, { "ebook", "e-book" }
        };

        private static readonly Dictionary<string, string> PortugueseNumberWords = new(StringComparer.OrdinalIgnoreCase)
        {
            { "um", "1" }, { "uma", "1" }, { "dois", "2" }, { "duas", "2" }, { "tres", "3" }, { "três", "3" },
            { "quatro", "4" }, { "cinco", "5" }, { "seis", "6" }, { "sete", "7" }, { "oito", "8" }, { "nove", "9" }, { "dez", "10" }
        };

        private static readonly HashSet<string> StopWords = new(StringComparer.OrdinalIgnoreCase)
        {
            "da","de","do","das","dos","a","o","e","para","por","na","no","em"
        };

        public static List<string> NormalizeTokens(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return new List<string>();

            string normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var ch in normalized)
            {
                var cat = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (cat != UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }
            string noDiacritics = sb.ToString().Normalize(NormalizationForm.FormC);

            sb.Clear();
            foreach (char c in noDiacritics)
            {
                if (char.IsLetterOrDigit(c))
                    sb.Append(char.ToLowerInvariant(c));
                else
                    sb.Append(' ');
            }

            var rawTokens = sb.ToString()
                              .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                              .Select(t => t.Trim())
                              .ToList();

            var outTokens = new List<string>();
            foreach (var token in rawTokens)
            {
                if (StopWords.Contains(token)) continue;

                int roman = RomanToInt(token);
                if (roman > 0)
                {
                    outTokens.Add(roman.ToString());
                    continue;
                }

                if (PortugueseNumberWords.TryGetValue(token, out var numWord))
                {
                    outTokens.Add(numWord);
                    continue;
                }

                if (Synonyms.TryGetValue(token, out var mapped))
                {
                    outTokens.Add(mapped);
                    continue;
                }

                if (int.TryParse(token, out _))
                {
                    outTokens.Add(token);
                    continue;
                }

                outTokens.Add(token);
            }

            var seen = new HashSet<string>();
            var distinct = new List<string>();
            foreach (var t in outTokens)
            {
                if (!seen.Contains(t))
                {
                    distinct.Add(t);
                    seen.Add(t);
                }
            }

            return distinct;
        }

        private static int RomanToInt(string s)
        {
            if (string.IsNullOrEmpty(s)) return 0;
            s = s.ToUpperInvariant();

            foreach (char c in s)
            {
                if ("MDCLXVI".IndexOf(c) < 0) return 0;
            }

            var map = new Dictionary<char, int> { { 'M', 1000 }, { 'D', 500 }, { 'C', 100 }, { 'L', 50 }, { 'X', 10 }, { 'V', 5 }, { 'I', 1 } };
            int total = 0;
            int prev = 0;
            for (int i = s.Length - 1; i >= 0; i--)
            {
                int val = map[s[i]];
                if (val < prev) total -= val;
                else { total += val; prev = val; }
            }
            return total;
        }

        public static double TokenJaccard(IEnumerable<string> a, IEnumerable<string> b)
        {
            var sa = new HashSet<string>(a);
            var sb = new HashSet<string>(b);
            if (sa.Count == 0 && sb.Count == 0) return 1.0;
            if (sa.Count == 0 || sb.Count == 0) return 0.0;
            var inter = sa.Intersect(sb).Count();
            var uni = sa.Union(sb).Count();
            return (double)inter / uni;
        }

        public static double LevenshteinSimilarity(string s, string t)
        {
            if (s == t) return 1.0;
            int d = LevenshteinDistance(s, t);
            int max = Math.Max(s.Length, t.Length);
            if (max == 0) return 1.0;
            return 1.0 - (double)d / max;
        }

        private static int LevenshteinDistance(string s, string t)
        {
            if (string.IsNullOrEmpty(s)) return t?.Length ?? 0;
            if (string.IsNullOrEmpty(t)) return s.Length;
            int n = s.Length, m = t.Length;
            var dp = new int[n + 1, m + 1];
            for (int i = 0; i <= n; i++) dp[i, 0] = i;
            for (int j = 0; j <= m; j++) dp[0, j] = j;
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = s[i - 1] == t[j - 1] ? 0 : 1;
                    dp[i, j] = Math.Min(Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1), dp[i - 1, j - 1] + cost);
                }
            }
            return dp[n, m];
        }

        private static double JaroWinkler(string s1, string s2)
        {
            double jaro = Jaro(s1, s2);
            // prefix length up to 4
            int prefix = 0;
            int maxPrefix = 4;
            int len = Math.Min(Math.Min(s1.Length, s2.Length), maxPrefix);
            for (int i = 0; i < len; i++)
            {
                if (s1[i] == s2[i]) prefix++;
                else break;
            }
            double p = 0.1;
            return jaro + prefix * p * (1 - jaro);
        }

        private static double Jaro(string s, string t)
        {
            if (s == t) return 1.0;
            int n = s.Length, m = t.Length;
            if (n == 0 || m == 0) return 0.0;
            int matchDist = Math.Max(n, m) / 2 - 1;
            if (matchDist < 0) matchDist = 0;

            var sMatches = new bool[n];
            var tMatches = new bool[m];

            int matches = 0;
            for (int i = 0; i < n; i++)
            {
                int start = Math.Max(0, i - matchDist);
                int end = Math.Min(m - 1, i + matchDist);
                for (int j = start; j <= end; j++)
                {
                    if (tMatches[j]) continue;
                    if (s[i] != t[j]) continue;
                    sMatches[i] = true;
                    tMatches[j] = true;
                    matches++;
                    break;
                }
            }
            if (matches == 0) return 0.0;

            int k = 0;
            int transpositions = 0;
            for (int i = 0; i < n; i++)
            {
                if (!sMatches[i]) continue;
                while (!tMatches[k]) k++;
                if (s[i] != t[k]) transpositions++;
                k++;
            }
            transpositions = transpositions / 2;
            double mD = matches;
            return (mD / n + mD / m + (mD - transpositions) / mD) / 3.0;
        }

        public static string FormatResult(string key, string value)
        {
            return $"[{key}] {value}";
        }
    }
}
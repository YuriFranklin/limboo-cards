using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using LimbooCards.Domain.Services;
using System.Text.Json;

namespace LimbooCards.Infra.Services;

public class ConceptNetSynonymProvider(HttpClient httpClient, IMemoryCache cache) : ISynonymProvider
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IMemoryCache _cache = cache;

    public async Task<IEnumerable<string>> GetSynonymsAsync(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
            return [];

        return await _cache.GetOrCreateAsync(
            $"conceptnet:{word.ToLowerInvariant()}",
            async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
                return await FetchFromConceptNet(word);
            }) ?? [];
    }

    private async Task<IEnumerable<string>> FetchFromConceptNet(string word)
    {
        var url = $"https://api.conceptnet.io/query?node=/c/pt/{Uri.EscapeDataString(word)}&rel=/r/Synonym&limit=5";
        try
        {
            var doc = await _httpClient.GetFromJsonAsync<JsonElement>(url);
            if (!doc.TryGetProperty("edges", out var edges)) return [];

            return [.. edges.EnumerateArray()
                        .Select(e =>
                            e.TryGetProperty("end", out var end) &&
                            end.TryGetProperty("label", out var label)
                                ? label.GetString()
                                : null)
                        .Where(s => !string.IsNullOrWhiteSpace(s) &&
                                    !string.Equals(s, word, StringComparison.OrdinalIgnoreCase))
                        .Select(s => s!)
                        .Distinct(StringComparer.OrdinalIgnoreCase)];
        }
        catch
        {
            return [];
        }
    }
}

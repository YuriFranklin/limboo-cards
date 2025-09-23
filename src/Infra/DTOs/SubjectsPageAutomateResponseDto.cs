namespace LimbooCards.Infra.DTOs
{
    using System.Text.Json.Serialization;
    public class SubjectsPageAutomateResponseDto
    {
        [JsonPropertyName("items")]
        public List<SubjectAutomateDto> Items { get; set; } = new();

        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage { get; set; }

        [JsonPropertyName("hasPreviousPage")]
        public bool HasPreviousPage { get; set; }
    }
}
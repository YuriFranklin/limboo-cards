namespace LimbooCards.Infra.DTOs
{
    using System.Text.Json.Serialization;
    public class ChecklistItemAutomateDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("value")]
        public ChecklistValue Value { get; set; } = null!;
    }

    public class ChecklistValue
    {
        [JsonPropertyName("isChecked")]
        public bool IsChecked { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("orderHint")]
        public string OrderHint { get; set; } = string.Empty;

        [JsonPropertyName("lastModifiedDateTime")]
        public DateTime LastModifiedDateTime { get; set; }

        [JsonPropertyName("lastModifiedBy")]
        public LastModifiedBy LastModifiedBy { get; set; } = null!;
    }

    public class LastModifiedBy
    {
        [JsonPropertyName("user")]
        public LastModifiedUser User { get; set; } = null!;
    }

    public class LastModifiedUser
    {
        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }

        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }
}
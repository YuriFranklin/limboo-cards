using System.Text.Json.Serialization;

namespace LimbooCards.Infra.DTOs
{
    public class CardAutomateDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = null!;

        [JsonPropertyName("planId")]
        public string PlanId { get; set; } = null!;

        [JsonPropertyName("bucketId")]
        public string BucketId { get; set; } = null!;

        [JsonPropertyName("orderHint")]
        public string OrderHint { get; set; } = null!;

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("hasDescription")]
        public bool HasDescription { get; set; } = false;

        [JsonPropertyName("subjectId")]
        public string SubjectId { get; set; } = string.Empty;

        [JsonPropertyName("appliedCategories")]
        public Dictionary<string, bool> AppliedCategories { get; set; } = new();

        [JsonPropertyName("createdDateTime")]
        public string CreatedDateTime { get; set; } = null!;

        [JsonPropertyName("dueDateTime")]
        public string? DueDateTime { get; set; }

        [JsonPropertyName("createdBy")]
        public CreatedBy CreatedBy { get; set; } = null!;

        [JsonPropertyName("checklist")]
        public List<ChecklistItemAutomateDto> Checklist { get; set; } = new();
    }

    public class CreatedBy
    {
        [JsonPropertyName("user")]
        public CreatedByUser User { get; set; } = null!;
    }

    public class CreatedByUser
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
    }
}
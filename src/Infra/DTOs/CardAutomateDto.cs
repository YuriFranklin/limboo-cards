namespace LimbooCards.Infra.DTOs
{
#pragma warning disable IDE1006
    public class CardAutomateDto
    {
        public string id { get; set; } = null!;
        public string planId { get; set; } = null!;
        public string bucketId { get; set; } = null!;
        public string orderHint { get; set; } = null!;
        public string? title { get; set; }
        public string? description { get; set; }
        public bool hasDescription { get; set; } = false;
        public string subjectId { get; set; } = string.Empty;
        public Dictionary<string, bool> appliedCategories { get; set; } = new();
        public string createdDateTime { get; set; } = null!;
        public string? dueDateTime { get; set; }
        public CreatedBy createdBy { get; set; } = null!;
        public List<ChecklistItemAutomateDto> checklist { get; set; } = new();
    }

    public class CreatedBy
    {
        public CreatedByUser user { get; set; } = null!;

    }

    public class CreatedByUser
    {
        public string id { get; set; } = string.Empty;
    }
}
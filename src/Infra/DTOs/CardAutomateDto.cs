namespace LimbooCards.Infra.DTOs
{
#pragma warning disable IDE1006
    public class CardAutomateDto
    {
        public Guid id { get; set; }
        public Guid planId { get; set; }
        public Guid bucketId { get; set; }
        public string orderHint { get; set; } = null!;
        public string? title { get; set; }
        public string? description { get; set; }
        public bool hasDescription { get; set; } = false;
        public Guid subjectId { get; set; }
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
        public Guid id { get; set; }
    }
}
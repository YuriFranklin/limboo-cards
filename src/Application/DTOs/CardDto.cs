namespace LimbooCards.Application.DTOs
{
    public class CardDto
    {
        public string? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool HasDescription { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? BucketId { get; set; }
        public string? PlanId { get; set; }
        public DateTime? DueDateTime { get; set; }
        public Dictionary<string, bool>? AppliedCategories { get; set; }
        public Guid? SubjectId { get; set; }
        public List<ChecklistItemDto>? Checklist { get; set; }
    }
}

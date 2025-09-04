namespace LimbooCards.Application.DTOs
{
    public class CardDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool HasDescription { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? BucketId { get; set; }
        public Guid? PlanId { get; set; }
        public DateTime? DueDateTime { get; set; }
        public Dictionary<string, bool>? AppliedCategories { get; set; }
        public Guid? SubjectId { get; set; }
        public List<ChecklistItemDto>? Checklist { get; set; }
    }
}

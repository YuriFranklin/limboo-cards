namespace LimbooCards.Application.DTOs
{
    public class CreateCardDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? BucketId { get; set; }
        public Guid? PlanId { get; set; }
        public DateTime? DueDateTime { get; set; }
        public Dictionary<string, bool>? AppliedCategories { get; set; }
        public List<ChecklistItemDto>? Checklist { get; set; }
    }
}

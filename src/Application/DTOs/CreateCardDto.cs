namespace LimbooCards.Application.DTOs
{
    public class CreateCardDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? BucketId { get; set; }
        public string? PlanId { get; set; } = string.Empty;
        public DateTime? DueDateTime { get; set; }
        public Dictionary<string, bool>? AppliedCategories { get; set; }
        public List<ChecklistItemDto>? Checklist { get; set; }
    }
}

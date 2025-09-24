namespace LimbooCards.Application.DTOs
{
    public class UpdateCardDto
    {
        public required string Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool HasDescription => !string.IsNullOrWhiteSpace(Description);
        public string? BucketId { get; set; }
        public string? PlanId { get; set; }
        public DateTime? DueDateTime { get; set; }
        public Dictionary<string, bool>? AppliedCategories { get; set; }
        public List<ChecklistItemDto>? Checklist { get; set; }
    }
}

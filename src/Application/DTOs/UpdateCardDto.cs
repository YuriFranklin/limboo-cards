namespace LimbooCards.Application.DTOs
{
    public class UpdateCardDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool HasDescription => !string.IsNullOrWhiteSpace(Description);
        public Guid? BucketId { get; set; }
        public Guid? PlanId { get; set; }
        public DateTime? DueDateTime { get; set; }
        public Dictionary<string, bool>? AppliedCategories { get; set; }
        public List<ChecklistItemDto>? Checklist { get; set; }
    }
}

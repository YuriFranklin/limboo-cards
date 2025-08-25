namespace LimbooCards.Application.DTOs
{
    public class ChecklistItemDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public bool IsChecked { get; set; }
        public string OrderHint { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}

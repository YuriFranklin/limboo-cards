namespace LimbooCards.Application.DTOs
{
    using LimbooCards.Domain.Entities;

    public class ContentDto
    {
        public string Name { get; set; } = string.Empty;
        public string ChecklistItemTitle { get; set; } = string.Empty;
        public int? Priority { get; private set; } = 0;
        public ContentStatus? ContentStatus { get; set; }
    }
}

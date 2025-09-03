namespace LimbooCards.Presentation.GraphQL.Models
{
    using LimbooCards.Domain.Shared;

    public class ContentModel
    {
        public string Name { get; set; } = string.Empty;
        public string ChecklistItemTitle { get; set; } = string.Empty;
        public ContentStatus? ContentStatus { get; set; }
        public int Priority { get; set; }
    }
}

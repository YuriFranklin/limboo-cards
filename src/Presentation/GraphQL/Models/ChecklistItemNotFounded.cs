namespace LimbooCards.Presentation.GraphQL.Models
{
    public class ChecklistItemNotFoundedModel
    {
        public Guid CardId { get; set; }
        public Guid SubjectId { get; set; }
        public string ChecklistItemId { get; set; } = string.Empty;
        public string ChecklistItemTitle { get; set; } = string.Empty;
    }
}

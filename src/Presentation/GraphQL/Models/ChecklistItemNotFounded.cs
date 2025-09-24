namespace LimbooCards.Presentation.GraphQL.Models
{
    public class ChecklistItemNotFoundedModel
    {
        public string CardId { get; set; } = string.Empty;
        public Guid SubjectId { get; set; }
        public string ChecklistItemId { get; set; } = string.Empty;
        public string ChecklistItemTitle { get; set; } = string.Empty;
    }
}

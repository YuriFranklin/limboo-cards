namespace LimbooCards.Presentation.GraphQL.Models
{
    public class ChecklistItemCompletedModel
    {
        public string ChecklistItemId { get; set; } = string.Empty;
        public Guid CompletedBy { get; set; }
        public DateTime CompletedAt { get; set; }
        public Guid CardId { get; set; }
        public Guid SubjectId { get; set; }
    }
}

namespace LimbooCards.Presentation.GraphQL.Models
{
    public class ChecklistItemCompletedModel
    {
        public string ChecklistItemId { get; set; } = string.Empty;
        public string CompletedBy { get; set; } = string.Empty;
        public DateTime CompletedAt { get; set; }
        public string CardId { get; set; } = string.Empty;
        public Guid SubjectId { get; set; }
    }
}

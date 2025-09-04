namespace LimbooCards.Presentation.GraphQL.Models
{
    public class ChecklistResultModel
    {
        public List<ChecklistItemCompletedModel> Completed { get; set; } = new();
        public List<ChecklistItemNotFoundedModel> NotFound { get; set; } = new();
    }
}
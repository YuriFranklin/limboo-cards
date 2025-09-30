namespace LimbooCards.Presentation.GraphQL.Contracts
{
    using LimbooCards.Presentation.GraphQL.Models;
    public class VerifyChecklistForCardsOutput
    {
        public List<ChecklistItemCompletedModel> Completed { get; set; } = new();
        public List<ChecklistItemNotFoundedModel> NotFound { get; set; } = new();
    }
}
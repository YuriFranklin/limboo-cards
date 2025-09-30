namespace LimbooCards.Presentation.GraphQL.Models
{
    public class NormalizeCardsResultModel
    {
        public List<CardModel> Success { get; set; } = new();
        public List<string> Failed { get; set; } = new();
    }
}
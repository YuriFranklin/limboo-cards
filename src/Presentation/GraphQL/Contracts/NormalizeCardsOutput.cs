namespace LimbooCards.Presentation.GraphQL.Contracts
{
    using LimbooCards.Presentation.GraphQL.Models;
    public class NormalizeCardsOutput
    {
        public List<CardModel> Success { get; set; } = new();
        public List<string> Failed { get; set; } = new();
    }
}
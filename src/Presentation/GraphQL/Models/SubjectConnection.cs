namespace LimbooCards.Presentation.GraphQL.Models
{
    public class SubjectConnection
    {
        public List<SubjectEdge> Edges { get; set; } = [];
        public PageInfo PageInfo { get; set; } = new();
    }
}
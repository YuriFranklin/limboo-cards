namespace LimbooCards.Presentation.GraphQL.Models
{
    public class SubjectEdge
    {
        public SubjectModel Node { get; set; } = default!;
        public string Cursor { get; set; } = string.Empty;
    }
}
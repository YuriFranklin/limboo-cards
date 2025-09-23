namespace LimbooCards.Presentation.GraphQL.Models
{
    public class SubjectEdgeModel
    {
        public SubjectModel? Node { get; set; }
        public string Cursor { get; set; } = string.Empty;
    }
}
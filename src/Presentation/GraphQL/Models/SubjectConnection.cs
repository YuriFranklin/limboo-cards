namespace LimbooCards.Presentation.GraphQL.Models
{
    public class SubjectConnectionModel
    {
        public List<SubjectEdgeModel> Edges { get; set; } = new();
        public PageInforModel PageInfor { get; set; } = new();
    }
}
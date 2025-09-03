namespace LimbooCards.Presentation.GraphQL.Models
{
    public class SubjectPublisherModel
    {
        public string Name { get; set; } = string.Empty;
        public bool? IsCurrent { get; set; }
        public bool? IsExpect { get; set; }
    }
}

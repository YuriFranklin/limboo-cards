namespace LimbooCards.Presentation.GraphQL.Models
{
    using LimbooCards.Presentation.Graphql.Models;
    public class SubjectModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public UserModel? Owner { get; set; }
        public string Semester { get; set; } = string.Empty;
        public string? Status { get; set; }
        public List<OfertModel> Oferts { get; set; } = new();
        public List<string>? Equivalencies { get; set; }
        public List<ContentModel>? Contents { get; set; }
        public List<UserModel>? CoOwners { get; set; }
        public List<SubjectPublisherModel>? Publishers { get; set; }
    }
}

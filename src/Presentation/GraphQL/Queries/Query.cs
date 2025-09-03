namespace LimbooCards.Presentation.GraphQL.Queries
{
    public class Query(SubjectQueries subjectQueries)
    {
        public SubjectQueries SubjectQueries { get; } = subjectQueries;
    }
}
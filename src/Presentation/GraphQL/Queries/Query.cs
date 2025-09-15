namespace LimbooCards.Presentation.GraphQL.Queries
{
    public class Query(SubjectQueries subjectQueries, CardQueries cardQueries)
    {
        public SubjectQueries SubjectQueries { get; } = subjectQueries;
        public CardQueries CardQueries { get; } = cardQueries;
    }
}
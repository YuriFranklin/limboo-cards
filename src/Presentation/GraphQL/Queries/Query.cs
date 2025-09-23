namespace LimbooCards.Presentation.GraphQL.Queries
{
    public class Query(SubjectQueries subjectQueries, CardQueries cardQueries)
    {
        public SubjectQueries Subject { get; } = subjectQueries;
        public CardQueries Card { get; } = cardQueries;
    }
}
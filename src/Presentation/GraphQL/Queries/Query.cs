namespace LimbooCards.Presentation.GraphQL.Queries
{
    public class Query(SubjectQueries subjectQueries, CardQueries cardQueries, PlannerQueries plannerQueries)
    {
        public SubjectQueries Subject { get; } = subjectQueries;
        public CardQueries Card { get; } = cardQueries;
        public PlannerQueries Planner { get; } = plannerQueries;

    }
}
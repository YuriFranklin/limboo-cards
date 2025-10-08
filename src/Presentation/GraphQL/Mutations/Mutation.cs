namespace LimbooCards.Presentation.GraphQL.Mutations
{
    public class Mutation(PlannerMutations plannerMutations, CardMutations cardMutations, SubjectMutations subjectMutations)
    {
        public PlannerMutations Planner { get; } = plannerMutations;
        public CardMutations Card { get; } = cardMutations;
        public SubjectMutations Subject { get; } = subjectMutations;
    }
}
namespace LimbooCards.Presentation.GraphQL.Mutations
{
    public class Mutation(PlannerMutations plannerMutations, CardMutations cardMutations)
    {
        public PlannerMutations Planner { get; } = plannerMutations;
        public CardMutations Card { get; } = cardMutations;
    }
}
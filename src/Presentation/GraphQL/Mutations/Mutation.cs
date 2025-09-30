namespace LimbooCards.Presentation.GraphQL.Mutations
{
    public class Mutation(PlannerMutations plannerMutations)
    {
        public PlannerMutations Planner { get; } = plannerMutations;

    }
}
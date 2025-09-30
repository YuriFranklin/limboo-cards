namespace LimbooCards.Presentation.GraphQL.Contracts
{
    using LimbooCards.Presentation.GraphQL.Models;
    public class CreatePlannerInput
    {
        public string Id { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public List<PlannerBucketModel> Buckets { get; private set; } = new();
        public List<PinRuleModel>? PinRules { get; private set; }
    }
}
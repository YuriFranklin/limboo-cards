namespace LimbooCards.Presentation.GraphQL.Types
{
    using LimbooCards.Presentation.GraphQL.Models;

    public class PlannerBucketType : ObjectType<PlannerBucketModel>
    {
        protected override void Configure(IObjectTypeDescriptor<PlannerBucketModel> descriptor)
        {
            descriptor.Name("PlannerBucket");
        }
    }
}

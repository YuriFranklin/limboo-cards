namespace LimbooCards.Presentation.GraphQL.Types
{
    using LimbooCards.Presentation.GraphQL.Models;

    public class PlannerType : ObjectType<PlannerModel>
    {
        protected override void Configure(IObjectTypeDescriptor<PlannerModel> descriptor)
        {
            descriptor.Name("Planner");
        }
    }
}

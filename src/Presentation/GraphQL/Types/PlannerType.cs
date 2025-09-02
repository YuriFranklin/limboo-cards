namespace LimbooCards.Presentation.GraphQL.Types
{
    public class PlannerType : ObjectType<Planner>
    {
        protected override void Configure(IObjectTypeDescriptor<Planner> descriptor)
        {
            descriptor.Field(f => f.Id).Type<NonNullType<UuidType>>();
            descriptor.Field(f => f.Name).Type<StringType>();
        }
    }
    public class Planner
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "Demo Planner";
    }
}

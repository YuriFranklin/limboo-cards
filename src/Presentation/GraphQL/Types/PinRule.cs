namespace LimbooCards.Presentation.GraphQL.Types
{
    using LimbooCards.Presentation.GraphQL.Models;

    public class PinRuleType : ObjectType<PinRuleModel>
    {
        protected override void Configure(IObjectTypeDescriptor<PinRuleModel> descriptor)
        {
            descriptor.Name("PinRule");
        }
    }
}

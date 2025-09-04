namespace LimbooCards.Presentation.GraphQL.Types
{
    using LimbooCards.Presentation.GraphQL.Models;

    public class ChecklistItemCompletedType : ObjectType<ChecklistItemCompletedModel>
    {
        protected override void Configure(IObjectTypeDescriptor<ChecklistItemCompletedModel> descriptor)
        {
            descriptor.Name("ChecklistItemCompleted");
        }
    }
}

namespace LimbooCards.Presentation.GraphQL.Types
{
    using LimbooCards.Presentation.GraphQL.Models;
    public class ChecklistItemNotFoundedType : ObjectType<ChecklistItemNotFoundedModel>
    {
        protected override void Configure(IObjectTypeDescriptor<ChecklistItemNotFoundedModel> descriptor)
        {
            descriptor.Name("ChecklistItemNotFounded");
        }
    }
}

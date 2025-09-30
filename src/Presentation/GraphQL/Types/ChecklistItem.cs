namespace LimbooCards.Presentation.GraphQL.Types
{
    using LimbooCards.Presentation.GraphQL.Models;

    public class ChecklistItemType : ObjectType<ChecklistItemModel>
    {
        protected override void Configure(IObjectTypeDescriptor<ChecklistItemModel> descriptor)
        {
            descriptor.Name("ChecklistItem");
        }
    }
}

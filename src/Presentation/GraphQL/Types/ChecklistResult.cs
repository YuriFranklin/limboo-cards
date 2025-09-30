namespace LimbooCards.Presentation.GraphQL.Types
{
    using LimbooCards.Presentation.GraphQL.Models;

    public class ChecklistResultType : ObjectType<ChecklistResultModel>
    {
        protected override void Configure(IObjectTypeDescriptor<ChecklistResultModel> descriptor)
        {
            descriptor.Name("ChecklistResult");
        }
    }
}

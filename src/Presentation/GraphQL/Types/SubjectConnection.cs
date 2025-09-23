namespace LimbooCards.Presentation.GraphQL.Types
{
    using LimbooCards.Presentation.GraphQL.Models;
    public class SubjectConnectionType : ObjectType<SubjectConnectionModel>
    {
        protected override void Configure(IObjectTypeDescriptor<SubjectConnectionModel> descriptor)
        {
            descriptor.Name("SubjectConnection");
        }
    }
}

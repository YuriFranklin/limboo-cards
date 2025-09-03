namespace LimbooCards.Presentation.GraphQL.Types
{
    using LimbooCards.Presentation.GraphQL.Models;
    public class SubjectType : ObjectType<SubjectModel>
    {
        protected override void Configure(IObjectTypeDescriptor<SubjectModel> descriptor)
        {
            descriptor.Name("Subject");
        }
    }
}

namespace LimbooCards.Presentation.GraphQL.Types
{
    using LimbooCards.Presentation.GraphQL.Models;
    public class SubjectPublisherType : ObjectType<SubjectPublisherModel>
    {
        protected override void Configure(IObjectTypeDescriptor<SubjectPublisherModel> descriptor)
        {
            descriptor.Name("SubjectPublisher");
        }
    }
}

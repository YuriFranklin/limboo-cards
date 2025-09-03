namespace LimbooCards.Presentation.GraphQL.Types
{
    using LimbooCards.Presentation.GraphQL.Models;
    public class ContentType : ObjectType<ContentModel>
    {
        protected override void Configure(IObjectTypeDescriptor<ContentModel> descriptor)
        {
            descriptor.Name("Content");
        }
    }
}

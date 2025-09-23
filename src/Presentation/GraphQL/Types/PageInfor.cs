namespace LimbooCards.Presentation.GraphQL.Types
{
    using LimbooCards.Presentation.GraphQL.Models;
    public class PageInforType : ObjectType<PageInforModel>
    {
        protected override void Configure(IObjectTypeDescriptor<PageInforModel> descriptor)
        {
            descriptor.Name("PageInfor");
        }
    }
}

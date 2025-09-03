namespace LimbooCards.Presentation.GraphQL.Types
{
    using LimbooCards.Presentation.GraphQL.Models;
    public class OfertType : ObjectType<OfertModel>
    {
         protected override void Configure(IObjectTypeDescriptor<OfertModel> descriptor)
        {
            descriptor.Name("Ofert");
        }
    }
}
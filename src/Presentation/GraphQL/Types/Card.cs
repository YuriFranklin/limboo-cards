namespace LimbooCards.Presentation.GraphQL.Types
{
    using LimbooCards.Presentation.GraphQL.Models;

    public class CardType : ObjectType<CardModel>
    {
        protected override void Configure(IObjectTypeDescriptor<CardModel> descriptor)
        {
            descriptor.Name("Card");
        }
    }
}

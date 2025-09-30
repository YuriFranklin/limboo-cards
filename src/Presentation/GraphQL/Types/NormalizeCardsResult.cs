namespace LimbooCards.Presentation.GraphQL.Types
{
    using LimbooCards.Presentation.GraphQL.Models;
    public class NormalizeCardsResultType : ObjectType<NormalizeCardsResultModel>
    {
        protected override void Configure(IObjectTypeDescriptor<NormalizeCardsResultModel> descriptor)
        {
            descriptor.Name("NormalizeCardsResult");
        }
    }
}

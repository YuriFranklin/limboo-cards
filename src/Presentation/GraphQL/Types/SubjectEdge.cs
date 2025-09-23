namespace LimbooCards.Presentation.GraphQL.Types
{
    using LimbooCards.Presentation.GraphQL.Models;
    public class SubjectEdgeType : ObjectType<SubjectEdgeModel>
    {
        protected override void Configure(IObjectTypeDescriptor<SubjectEdgeModel> descriptor)
        {
            descriptor.Name("SubjectEdge");
        }
    }
}

namespace LimbooCards.Presentation.GraphQL.Types
{
    using LimbooCards.Presentation.Graphql.Models;
    public class UserType : ObjectType<UserModel> {
        protected override void Configure(IObjectTypeDescriptor<UserModel> descriptor)
        {
            descriptor.Name("User");
        }
    }
}

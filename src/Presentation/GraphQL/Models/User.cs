namespace LimbooCards.Presentation.GraphQL.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
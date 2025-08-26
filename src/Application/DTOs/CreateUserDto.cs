namespace LimbooCards.Application.DTOs
{
    public class CreateUserDto
    {
        public Guid? Id { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
}

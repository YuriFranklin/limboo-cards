namespace LimbooCards.Application.DTOs
{
    public class CreateUserDto
    {
        public string? Id { get; set; }
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}

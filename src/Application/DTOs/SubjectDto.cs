namespace LimbooCards.Application.DTOs
{
    using LimbooCards.Domain.Shared;

    public class SubjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Semester { get; set; } = string.Empty;
        public SubjectStatus Status { get; set; }
        public List<OfertDto> Oferts { get; set; } = new();
        public List<string>? Equivalencies { get; set; }
        public List<ContentDto>? Contents { get; set; }
        public UserDto? Owner { get; set; }
        public List<UserDto>? CoOwners { get; set; }
        public List<SubjectPublisherDto>? Publishers { get; set; }
    }
}

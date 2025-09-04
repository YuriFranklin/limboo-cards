namespace LimbooCards.Application.DTOs
{
    using LimbooCards.Domain.Events;
    public class ChecklistResultDto
    {
        public List<ChecklistItemCompleted>? Completed { get; set; }
        public List<ChecklistItemNotFounded>? NotFound { get; set; }
    }
}
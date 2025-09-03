namespace LimbooCards.Domain.Services
{
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Shared;

    public class SubjectCardOrchestratorService
    {
        public static Card? EnsureCardForSubject(Subject subject, Planner planner)
        {
            if (subject.Contents == null || subject.Contents.Count == 0 || !subject.Contents.Exists(c => c.ContentStatus == ContentStatus.Missing))
            {
                return null;
            }

            var checklist = subject.Contents
            .Select((content, index) => new { content, index })
            .Where(x => x.content.ContentStatus == ContentStatus.Missing)
            .Select(x => new ChecklistItem(
                id: (x.index + 1).ToString(),
                title: x.content.Name,
                isChecked: false,
                orderHint: "0000000000",
                updatedAt: DateTime.UtcNow,
                updatedBy: subject.Owner?.Id ?? Guid.Empty
            ))
            .ToList();

            var card = new Card(
                title: $"[PENDÊNCIA] {subject.Name}",
                hasDescription: false,
                createdBy: subject.Owner?.Id ?? Guid.Empty,
                createdAt: DateTime.UtcNow,
                checklist: checklist
            );

            return card;
        }
    }
}
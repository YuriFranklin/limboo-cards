/* namespace LimbooCards.Domain.Services
{
    using LimbooCards.Domain.Entities;

    public class SubjectCardOrchestratorService
    {
        public Card? EnsureCardForSubject(Subject subject, Planner planner)
        {
            if (subject.Contents == null || subject.Contents.Count == 0 || !subject.Contents.Exists(c => c.ContentStatus == ContentStatus.Missing))
            {
                return null;
            }

            var card = new Card(
                title: $"[PENDÊNCIA] {subject.Name}",
                hasDescription: false,
                createdBy: subject.Owner?.Id ?? Guid.Empty,
                createdAt: DateTime.UtcNow
            );
        }
    }
} */
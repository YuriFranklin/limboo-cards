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
                title: x.content.ChecklistItemTitle,
                isChecked: false,
                orderHint: x.index.ToString(),
                updatedAt: DateTime.UtcNow,
                updatedBy: subject.Owner?.Id ?? string.Empty
            ))
            .ToList();

            var card = new Card(
                title: CardTitleNormalizeService.Normalize(subject),
                hasDescription: false,
                planId: planner.Id,
                createdBy: subject.Owner?.Id ?? string.Empty,
                createdAt: DateTime.UtcNow,
                checklist: checklist,
                subjectId: subject.Id,
                appliedCategories: PinEvaluatorService.EvaluateCardPins(subject, planner, null)?.AppliedCategories
            );

            return card;
        }
    }
}
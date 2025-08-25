using LimbooCards.Domain.Entities;
using LimbooCards.Domain.Events;

namespace LimbooCards.Domain.Services
{
    public class ChecklistComparisonService
    {
        public List<ChecklistItemCompleted> GetCompletedChecklistItems(Card card, Subject subject)
        {
            if (card.Checklist == null || subject.Contents == null)
            {
                return new List<ChecklistItemCompleted>();
            }

            var completedItems = new List<ChecklistItemCompleted>();

            foreach (var item in card.Checklist)
            {
                if (subject.Contents.Any(c => c.ChecklistItemTitle == item.Title && c.ContentStatus == ContentStatus.Missing))
                {
                    completedItems.Add(new ChecklistItemCompleted(
                        cardId: card.Id,
                        subjectId: subject.Id,
                        checklistItemId: item.Id,
                        completedBy: card.CreatedBy,
                        completedAt: DateTime.UtcNow
                    ));
                }
            }

            return completedItems;
        }

        public List<ChecklistItemNotFounded> GetNotFoundChecklistItems(Card card, Subject subject)
        {
            if (subject.Contents == null || !subject.Contents.Any(c => c.ContentStatus == ContentStatus.Missing))
            {
                return new List<ChecklistItemNotFounded>();
            }

            var notFoundItems = new List<ChecklistItemNotFounded>();

            int maxId = 0;
            if (card.Checklist != null && card.Checklist.Any())
            {
                maxId = card.Checklist
                    .Select(ci => int.TryParse(ci.Id, out var n) ? n : 0)
                    .DefaultIfEmpty(0)
                    .Max();
            }

            foreach (var content in subject.Contents.Where(c => c.ContentStatus == ContentStatus.Missing))
            {
                if (card.Checklist == null || !card.Checklist.Any(ci => ci.Title == content.ChecklistItemTitle))
                {
                    maxId++;

                    notFoundItems.Add(new ChecklistItemNotFounded(
                        checklistItemId: maxId.ToString(),
                        checklistItemTitle: content.ChecklistItemTitle,
                        cardId: card.Id,
                        subjectId: subject.Id
                    ));
                }
            }

            return notFoundItems;
        }
    }
}
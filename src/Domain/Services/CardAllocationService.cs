namespace LimbooCards.Domain.Services
{
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Events;
    using LimbooCards.Domain.Shared;

    public class CardAllocationService
    {
        public static CardPlannerAllocated? AllocateCardToBucket(Card card, Subject subject, Planner planner)
        {
            if (string.IsNullOrWhiteSpace(card.Id))
                return null;

            if (card.Checklist == null || card.Checklist.Count == 0)
                return null;

            bool hasNoMissingContent = subject.Contents == null || !subject.Contents.Any(c => c.ContentStatus == ContentStatus.Missing);

            var currentBucket = planner.Buckets.FirstOrDefault(b => b.Id == card.BucketId);

            if (currentBucket != null && currentBucket.IsEnd && hasNoMissingContent)
            {
                return new CardPlannerAllocated(card.Id, planner.Id, currentBucket.Id);
            }

            if (hasNoMissingContent)
            {
                return null;
            }

            var bucket = GetBestBucket(subject, planner)
                         ?? throw new InvalidOperationException("No suitable bucket found and no default bucket available.");

            return new CardPlannerAllocated(
                card.Id,
                planner.Id,
                bucket.Id
            );
        }

        private static PlannerBucket? GetBestBucket(Subject subject, Planner planner)
        {
            return subject.Contents!
                .Where(c => c.ContentStatus == ContentStatus.Missing)
                .OrderByDescending(c => c.Priority)
                .SelectMany(c => planner.Buckets
                    .Where(b => b.Name.Contains(c.Name, StringComparison.OrdinalIgnoreCase)))
                .FirstOrDefault()
                ?? planner.Buckets.FirstOrDefault(b => b.IsDefault);
        }
    }
}
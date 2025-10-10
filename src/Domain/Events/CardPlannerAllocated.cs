namespace LimbooCards.Domain.Events
{
    public class CardPlannerAllocated
    {
        public CardPlannerAllocated(string? cardId, string plannerId, string bucketId)
        {
            CardId = cardId;
            PlannerId = plannerId;
            BucketId = bucketId;

            Validate();
        }
        public string? CardId { get; }
        public string PlannerId { get; }
        public string BucketId { get; }
        private void Validate()
        {
            if (PlannerId == null || PlannerId == string.Empty)
                throw new ArgumentException("PlannerId cannot be null or empty.", nameof(PlannerId));

            if (BucketId == null || BucketId == string.Empty)
                throw new ArgumentException("BucketId cannot be null or empty.", nameof(BucketId));
        }
    }
}

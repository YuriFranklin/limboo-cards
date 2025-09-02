namespace LimbooCards.Domain.Events
{
    public class CardPlannerAllocated
    {
        public CardPlannerAllocated(Guid cardId, Guid plannerId, Guid bucketId)
        {
            CardId = cardId;
            PlannerId = plannerId;
            BucketId = bucketId;

            Validate();
        }
        public Guid CardId { get; }
        public Guid PlannerId { get; }
        public Guid BucketId { get; }
        private void Validate()
        {
            if (CardId == Guid.Empty)
                throw new ArgumentException("CardId cannot be empty.", nameof(CardId));

            if (PlannerId == Guid.Empty)
                throw new ArgumentException("PlannerId cannot be empty.", nameof(PlannerId));

            if (BucketId == Guid.Empty)
                throw new ArgumentException("BucketId cannot be empty.", nameof(BucketId));
        }
    }
}

namespace LimbooCards.Domain.Events
{
    public class ChecklistItemNormalized
    {
        public string CardId { get; private set; }
        public string ChecklistItemId { get; private set; }
        public string NormalizedTitle { get; private set; }

        public ChecklistItemNormalized(string cardId, string checklistItemId, string normalizedTitle)
        {
            CardId = cardId;
            ChecklistItemId = checklistItemId;
            NormalizedTitle = normalizedTitle;
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(CardId))
                throw new ArgumentException("CardId cannot be empty.", nameof(CardId));

            if (string.IsNullOrWhiteSpace(ChecklistItemId))
                throw new ArgumentException("ChecklistItemId cannot be empty.", nameof(ChecklistItemId));

            if (string.IsNullOrWhiteSpace(NormalizedTitle))
                throw new ArgumentException("NormalizedTitle cannot be empty.", nameof(NormalizedTitle));
        }
    }
}
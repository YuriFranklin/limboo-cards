namespace LimbooCards.Domain.Events
{
    public class ChecklistItemCompleted
    {
        public ChecklistItemCompleted(
            string checklistItemId,
            string completedBy,
            DateTime completedAt,
            string cardId,
            Guid subjectId)
        {
            this.ChecklistItemId = checklistItemId;
            this.CompletedBy = completedBy;
            this.CompletedAt = completedAt;
            this.CardId = cardId;
            this.SubjectId = subjectId;

            this.Validate();
        }

        public string ChecklistItemId { get; private set; }
        public string CompletedBy { get; private set; }
        public DateTime CompletedAt { get; private set; }
        public string CardId { get; private set; }
        public Guid SubjectId { get; private set; }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.ChecklistItemId))
                throw new ArgumentException("ChecklistItemId must be set.", nameof(this.ChecklistItemId));

            if (string.IsNullOrWhiteSpace(this.CompletedBy))
                throw new ArgumentException("CompletedBy must be set.", nameof(this.CompletedBy));

            if (this.CompletedAt == default)
                throw new ArgumentException("CompletedAt must be set.", nameof(this.CompletedAt));

            if (CardId == null || this.CardId == string.Empty)
                throw new ArgumentException("CardId must be set.", nameof(this.CardId));

            if (this.SubjectId == Guid.Empty)
                throw new ArgumentException("SubjectId must be set.", nameof(this.SubjectId));
        }
    }
}

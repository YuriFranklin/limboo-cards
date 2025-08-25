namespace LimbooCards.Domain.Events
{
    public class ChecklistItemNotFounded
    {
        public ChecklistItemNotFounded(string checklistItemId, string checklistItemTitle, Guid cardId, Guid subjectId)
        {
            this.ChecklistItemId = checklistItemId;
            this.ChecklistItemTitle = checklistItemTitle;
            this.CardId = cardId;
            this.SubjectId = subjectId;
            this.Validate();
        }

        public Guid CardId { get; private set; }
        public Guid SubjectId { get; private set; }
        public string ChecklistItemId { get; private set; }
        public string ChecklistItemTitle { get; private set; }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.ChecklistItemId))
            {
                throw new ArgumentException("ChecklistItemId cannot be empty.", nameof(this.ChecklistItemId));
            }
            if (string.IsNullOrWhiteSpace(this.ChecklistItemTitle))
            {
                throw new ArgumentException("ChecklistItemTitle cannot be empty.", nameof(this.ChecklistItemTitle));
            }

            if (this.CardId == Guid.Empty)
            {
                throw new ArgumentException("CardId must be set.", nameof(this.CardId));
            }

            if (this.SubjectId == Guid.Empty)
            {
                throw new ArgumentException("SubjectId must be set.", nameof(this.SubjectId));
            }
        }
    }
}

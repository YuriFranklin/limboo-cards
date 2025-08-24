namespace LimbooCards.Domain.Entities
{
    public class ChecklistItem
    {
        public ChecklistItem(
            string title,
            bool isChecked,
            string orderHint,
            DateTime updatedAt,
            Guid updatedBy)
        {
            this.Title = title;
            this.IsChecked = isChecked;
            this.OrderHint = orderHint;
            this.UpdatedAt = updatedAt;
            this.UpdatedBy = updatedBy;

            this.Validate();
        }

        public string Title { get; private set; }
        public bool IsChecked { get; private set; }
        public string OrderHint { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public Guid UpdatedBy { get; private set; }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.Title))
            {
                throw new ArgumentException("Title cannot be empty.", nameof(this.Title));
            }

            if (string.IsNullOrWhiteSpace(this.OrderHint))
            {
                throw new ArgumentException("OrderHint cannot be empty.", nameof(this.OrderHint));
            }

            if (this.UpdatedAt == default)
            {
                throw new ArgumentException("UpdatedAt must be set.", nameof(this.UpdatedAt));
            }

            if (this.UpdatedBy == Guid.Empty)
            {
                throw new ArgumentException("UpdatedBy must be set.", nameof(this.UpdatedBy));
            }
        }
    }
}

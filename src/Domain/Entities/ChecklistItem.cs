namespace LimbooCards.Domain.Entities
{
    public class ChecklistItem
    {
        public ChecklistItem(
            string id,
            string title,
            bool isChecked,
            string orderHint,
            DateTime updatedAt,
            string updatedBy)
        {
            this.Id = id;
            this.Title = title;
            this.IsChecked = isChecked;
            this.OrderHint = orderHint;
            this.UpdatedAt = updatedAt;
            this.UpdatedBy = updatedBy;

            this.Validate();
        }

        public string Id { get; private set; }
        public string Title { get; private set; }
        public bool IsChecked { get; private set; }
        public string OrderHint { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public string UpdatedBy { get; private set; }

        public ChecklistItem With(
            string? id = null,
            string? title = null,
            bool? isChecked = null,
            string? orderHint = null,
            DateTime? updatedAt = null,
            string? updatedBy = null
        )
        {
            return new ChecklistItem(
                id ?? this.Id,
                title ?? this.Title,
                isChecked ?? this.IsChecked,
                orderHint ?? this.OrderHint,
                updatedAt ?? this.UpdatedAt,
                updatedBy ?? this.UpdatedBy
            );
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.Id))
            {
                throw new ArgumentException("Id cannot be empty.", nameof(this.Id));
            }
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

            if (string.IsNullOrWhiteSpace(UpdatedBy))
            {
                throw new ArgumentException("UpdatedBy must be set.", nameof(this.UpdatedBy));
            }
        }
    }
}

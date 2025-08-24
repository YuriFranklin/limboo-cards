namespace LimbooCards.Domain.Entities
{
    public class Card
    {
        public Card(
            string title,
            string description,
            bool hasDescription,
            Guid createdBy,
            Guid? id = null,
            Guid? bucketId = null,
            Guid? planId = null,
            DateTime? createdAt = null,
            DateTime? dueDateTime = null,
            Dictionary<string, bool>? appliedCategories = null,
            List<ChecklistItem>? checklist = null
        )
        {
            this.Id = id ?? Guid.NewGuid();
            this.Title = title;
            this.Description = description;
            this.HasDescription = hasDescription;
            this.CreatedBy = createdBy;
            this.CreatedAt = createdAt ?? DateTime.UtcNow;

            this.BucketId = bucketId;
            this.PlanId = planId;
            this.DueDateTime = dueDateTime;
            this.AppliedCategories = appliedCategories;
            this.Checklist = checklist;

            this.Validate();
        }

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool HasDescription { get; private set; }
        public Guid CreatedBy { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Guid? BucketId { get; private set; }
        public Guid? PlanId { get; private set; }
        public DateTime? DueDateTime { get; private set; }
        public Dictionary<string, bool>? AppliedCategories { get; private set; }
        public List<ChecklistItem>? Checklist { get; private set; }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.Title))
            {
                throw new ArgumentException("Title cannot be empty.", nameof(this.Title));
            }

            if (this.DueDateTime.HasValue && this.DueDateTime.Value < this.CreatedAt)
            {
                throw new ArgumentException("Due date cannot be earlier than the creation date.", nameof(this.DueDateTime));
            }
        }
    }
}

namespace LimbooCards.Domain.Entities
{
    using System.Text.Json.Serialization;

    public class Card
    {
        public Card(
            string title,
            bool hasDescription,
            string createdBy,
            string planId,
            string? description = null,
            string? id = null,
            string? bucketId = null,
            Guid? subjectId = null,
            DateTime? createdAt = null,
            DateTime? dueDateTime = null,
            Dictionary<string, bool>? appliedCategories = null,
            List<ChecklistItem>? checklist = null
        )
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.HasDescription = hasDescription;
            this.CreatedBy = createdBy;
            this.CreatedAt = createdAt ?? DateTime.UtcNow;
            this.SubjectId = subjectId;
            this.BucketId = bucketId;
            this.PlanId = planId;
            this.DueDateTime = dueDateTime;
            this.AppliedCategories = appliedCategories;
            this.Checklist = checklist;

            this.Validate();
        }

        [JsonConstructor]
        internal Card() { }

        [JsonInclude] public string? Id { get; private set; }
        [JsonInclude] public string Title { get; private set; } = null!;
        [JsonInclude] public string? Description { get; private set; }
        [JsonInclude] public bool HasDescription { get; private set; }
        [JsonInclude] public string CreatedBy { get; private set; } = null!;
        [JsonInclude] public DateTime CreatedAt { get; private set; }
        [JsonInclude] public string? BucketId { get; private set; }
        [JsonInclude] public string PlanId { get; private set; } = null!;
        [JsonInclude] public Guid? SubjectId { get; private set; }
        [JsonInclude] public DateTime? DueDateTime { get; private set; }
        [JsonInclude] public Dictionary<string, bool>? AppliedCategories { get; private set; }
        [JsonInclude] public List<ChecklistItem>? Checklist { get; private set; }

        public Card With(
            string? title = null,
            bool? hasDescription = null,
            string? createdBy = null,
            string? description = null,
            string? id = null,
            string? bucketId = null,
            string? planId = null,
            Guid? subjectId = null,
            DateTime? createdAt = null,
            DateTime? dueDateTime = null,
            Dictionary<string, bool>? appliedCategories = null,
            List<ChecklistItem>? checklist = null)
        {
            return new Card(
                title ?? this.Title,
                hasDescription ?? this.HasDescription,
                createdBy ?? this.CreatedBy,
                planId ?? this.PlanId,
                description ?? this.Description,
                id ?? this.Id,
                bucketId ?? this.BucketId,
                subjectId ?? this.SubjectId,
                createdAt ?? this.CreatedAt,
                dueDateTime ?? this.DueDateTime,
                appliedCategories ?? this.AppliedCategories,
                checklist ?? this.Checklist
            );
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.Title))
            {
                throw new ArgumentException("Title cannot be empty.", nameof(this.Title));
            }

            if (string.IsNullOrWhiteSpace(this.Id) && this.DueDateTime.HasValue && this.DueDateTime.Value < this.CreatedAt)
            {
                throw new ArgumentException("Due date cannot be earlier than the creation date.", nameof(this.DueDateTime));
            }
        }
    }
}

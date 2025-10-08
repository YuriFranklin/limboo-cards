namespace LimbooCards.Presentation.GraphQL.Models
{
    using System;
    using System.Collections.Generic;

    public class CardModel
    {
        public string? Id { get; set; } = string.Empty;
        public string PlanId { get; set; } = string.Empty;
        public string BucketId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool HasDescription { get; set; }
        public Guid? SubjectId { get; set; }
        public string? OrderHint { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDateTime { get; set; }
        public string? CreatedBy { get; set; }
        public Dictionary<string, bool>? AppliedCategories { get; set; }
        public List<ChecklistItemModel>? Checklist { get; set; }
    }
}

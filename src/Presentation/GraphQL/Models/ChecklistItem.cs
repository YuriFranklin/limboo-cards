namespace LimbooCards.Presentation.GraphQL.Models
{
    using System;

    public class ChecklistItemModel
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public bool IsChecked { get; set; }
        public string? OrderHint { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public UserModel? LastModifiedBy { get; set; }
    }
}

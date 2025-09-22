namespace LimbooCards.Domain.Entities
{
    using System.Text.Json.Serialization;
    using LimbooCards.Domain.Shared;

    public class Content
    {
        public Content(string name, string checklistItemTitle, ContentStatus? status = null, int? priority = null)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.ChecklistItemTitle = checklistItemTitle ?? throw new ArgumentNullException(nameof(checklistItemTitle));
            this.ContentStatus = status;
            this.Priority = priority ?? 1;

            Validate();
        }

        [JsonConstructor]
        internal Content() { }

        [JsonInclude] public string Name { get; private set; } = null!;
        [JsonInclude] public string ChecklistItemTitle { get; private set; } = null!;
        [JsonInclude] public ContentStatus? ContentStatus { get; private set; }
        [JsonInclude] public int Priority { get; private set; } = 1;

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("Name cannot be empty.", nameof(Name));

            if (string.IsNullOrWhiteSpace(ChecklistItemTitle))
                throw new ArgumentException("ChecklistItemTitle cannot be empty.", nameof(ChecklistItemTitle));
        }
    }
}

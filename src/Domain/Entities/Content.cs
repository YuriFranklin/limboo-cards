namespace LimbooCards.Domain.Entities
{
    public class Content
    {
        public Content(string name, string checklistItemTitle, ContentStatus? status = null, int? priority = null)
        {
            this.Name = name;
            this.ChecklistItemTitle = checklistItemTitle;
            this.ContentStatus = status;
            this.Priority = priority ?? 1;
            this.Validate();
        }

        public string Name { get; private set; }
        public string ChecklistItemTitle { get; private set; }
        public ContentStatus? ContentStatus { get; private set; }
        public int Priority { get; private set; }
        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                throw new ArgumentException("Name cannot be empty.", nameof(this.Name));
            }

            if (string.IsNullOrWhiteSpace(this.ChecklistItemTitle))
            {
                throw new ArgumentException("ChecklistItemTitle cannot be empty.", nameof(this.ChecklistItemTitle));
            }
        }
    }
}

namespace LimbooCards.Domain.Entities
{
    using OneOf;

    public class Content
    {
        public Content(string name, string checklistItemTitle, OneOf<string, bool> value)
        {
            this.Name = name;
            this.ChecklistItemTitle = checklistItemTitle;
            this.Value = value;

            this.Validate();
        }

        public string Name { get; private set; }
        public string ChecklistItemTitle { get; private set; }
        public OneOf<string, bool> Value { get; private set; }

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

            if (this.Value.IsT0 && string.IsNullOrWhiteSpace(this.Value.AsT0))
            {
                throw new ArgumentException("Value (string) cannot be empty.", nameof(this.Value));
            }
        }
    }
}

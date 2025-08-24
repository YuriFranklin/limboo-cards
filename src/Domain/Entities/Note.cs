namespace LimbooCards.Domain.Entities
{
    public class Note
    {
        public Note(
            string text,
            Guid createdBy,
            DateTime? createdAt = null,
            DateTime? editedAt = null
        )
        {
            this.Text = text;
            this.CreatedBy = createdBy;
            this.CreatedAt = createdAt ?? DateTime.UtcNow;
            this.EditedAt = editedAt;

            this.Validate();
        }

        public string Text { get; private set; }
        public Guid CreatedBy { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? EditedAt { get; private set; }

        public void Edit(string newText)
        {
            if (string.IsNullOrWhiteSpace(newText))
            {
                throw new ArgumentException("Text cannot be empty.", nameof(newText));
            }

            this.Text = newText;
            this.EditedAt = DateTime.UtcNow;
        }
        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.Text))
            {
                throw new ArgumentException("Text cannot be empty.", nameof(this.Text));
            }

            if (this.CreatedBy == Guid.Empty)
            {
                throw new ArgumentException("CreatedBy must be set.", nameof(this.CreatedBy));
            }
        }


    }
}

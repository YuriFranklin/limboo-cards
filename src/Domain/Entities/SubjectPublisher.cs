namespace LimbooCards.Domain.Entities
{
    public class SubjectPublisher
    {
        public SubjectPublisher(string name, bool? isCurrent = false, bool? isExpect = false)
        {
            this.Name = name;
            this.IsCurrent = isCurrent;
            this.IsExpect = isExpect;

            this.Validate();
        }

        public string Name { get; set; }
        public bool? IsCurrent { get; set; }
        public bool? IsExpect { get; set; }
        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                throw new ArgumentException("Name cannot be empty.", nameof(this.Name));
            }
        }
    }
}
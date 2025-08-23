namespace LimbooCards.Domain.Entities
{
    public class Subject
    {
        public Subject(Guid? id, string name)
        {
            this.Id = id ?? Guid.NewGuid();
            this.Name = name;

            this.Validate();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public List<string>? Equivalencies { get; private set; }
        public SubjectStatus Status { get; set; }
        private void Validate()
        {
            if (this.Id == Guid.Empty)
            {
                throw new ArgumentException("Id must be set.", nameof(this.Id));
            }

            if (string.IsNullOrWhiteSpace(this.Name))
            {
                throw new ArgumentException("Name cannot be empty.", nameof(this.Name));
            }
        }
    }
}

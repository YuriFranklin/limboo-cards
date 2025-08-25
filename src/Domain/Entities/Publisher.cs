namespace LimbooCards.Domain.Entities
{
    public class Publisher
    {
        public Publisher(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;

            this.Validate();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }

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
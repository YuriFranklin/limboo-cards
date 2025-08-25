namespace LimbooCards.Domain.Entities
{
    public class User
    {
        public User(Guid id, string fullName)
        {
            this.Id = id;
            this.FullName = fullName;

            this.Validate();
        }

        public Guid Id { get; private set; }
        public string FullName { get; private set; }

        private void Validate()
        {
            if (this.Id == Guid.Empty)
            {
                throw new ArgumentException("Id must be set.", nameof(this.Id));
            }

            if (string.IsNullOrWhiteSpace(this.FullName))
            {
                throw new ArgumentException("FullName cannot be empty.", nameof(this.FullName));
            }
        }
    }
}
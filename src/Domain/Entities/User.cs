namespace LimbooCards.Domain.Entities
{
    public class User
    {
        public User(Guid id, string username)
        {
            this.Id = id;
            this.Username = username;

            this.Validate();
        }

        public Guid Id { get; private set; }
        public string Username { get; private set; }

        private void Validate()
        {
            if (this.Id == Guid.Empty)
            {
                throw new ArgumentException("Id must be set.", nameof(this.Id));
            }

            if (string.IsNullOrWhiteSpace(this.Username))
            {
                throw new ArgumentException("Username cannot be empty.", nameof(this.Username));
            }
        }
    }
}
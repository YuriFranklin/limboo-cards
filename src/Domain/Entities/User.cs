namespace LimbooCards.Domain.Entities
{
    public class User
    {
        public User(string id, string name, string fullName, string email)
        {
            this.Id = id;
            this.Name = name;
            this.FullName = fullName;
            this.Email = email;

            this.Validate();
        }

        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        private void Validate()
        {
            if (this.Id == null || this.Id == string.Empty)
            {
                throw new ArgumentException("Id must be set.", nameof(this.Id));
            }

            if (string.IsNullOrWhiteSpace(this.Name))
            {
                throw new ArgumentException("Name cannot be empty.", nameof(this.Name));
            }

            if (string.IsNullOrWhiteSpace(this.FullName))
            {
                throw new ArgumentException("FullName cannot be empty.", nameof(this.FullName));
            }

            if (string.IsNullOrWhiteSpace(this.Email))
            {
                throw new ArgumentException("Email cannot be empty.", nameof(this.Email));
            }
        }
    }
}
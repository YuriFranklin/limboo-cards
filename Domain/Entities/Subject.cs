namespace LimbooCards.Domain.Entities
{
    public class Subject
    {
        public Subject(
            Guid? id,
            string name,
            string semester,
            SubjectStatus status,
            List<string>? equivalencies = null,
            List<Content>? contents = null
        )
        {
            this.Id = id ?? Guid.NewGuid();
            this.Name = name;
            this.Semester = semester;
            this.Status = status;
            this.Equivalencies = equivalencies;
            this.Contents = contents;

            this.Validate();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Semester { get; private set; }
        public SubjectStatus Status { get; private set; }

        public List<string>? Equivalencies { get; private set; }
        public List<Content>? Contents { get; private set; }

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

            if (string.IsNullOrWhiteSpace(this.Semester))
            {
                throw new ArgumentException("Semester cannot be empty.", nameof(this.Semester));
            }

            if (!Enum.IsDefined(typeof(SubjectStatus), this.Status))
            {
                throw new ArgumentException("Invalid status value.", nameof(this.Status));
            }

            if (this.Equivalencies != null && this.Equivalencies.Any(e => string.IsNullOrWhiteSpace(e)))
            {
                throw new ArgumentException("Equivalencies cannot contain empty values.", nameof(this.Equivalencies));
            }

            if (this.Contents != null && this.Contents.Any(c => c is null))
            {
                throw new ArgumentException("Contents cannot contain null values.", nameof(this.Contents));
            }
        }
    }
}

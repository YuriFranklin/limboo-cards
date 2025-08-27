namespace LimbooCards.Domain.Entities
{
    public class Subject
    {
        public Subject(
            Guid? id,
            string name,
            string semester,
            SubjectStatus? status,
            List<Ofert>? oferts = null,
            List<string>? equivalencies = null,
            List<Content>? contents = null,
            User? owner = null,
            List<User>? coOwners = null,
            List<SubjectPublisher>? publishers = null,
            bool? isCurrent = null,
            bool? isExpect = null
        )
        {
            this.Id = id ?? Guid.NewGuid();
            this.Name = name;
            this.Semester = semester;
            this.Status = status;
            this.Oferts = oferts ?? new List<Ofert>();
            this.Equivalencies = equivalencies;
            this.Contents = contents;
            this.Owner = owner;
            this.CoOwners = coOwners;
            this.Publishers = publishers;
            this.IsCurrent = isCurrent;
            this.IsExpect = isExpect;

            this.Validate();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public User? Owner { get; private set; }
        public string Semester { get; private set; }
        public SubjectStatus? Status { get; private set; }
        public List<Ofert> Oferts { get; private set; }
        public List<string>? Equivalencies { get; private set; }
        public List<Content>? Contents { get; private set; }
        public List<User>? CoOwners { get; private set; }
        public List<SubjectPublisher>? Publishers { get; private set; }
        public bool? IsCurrent { get; private set; }
        public bool? IsExpect { get; private set; }

        private void Validate()
        {
            if (this.Id == Guid.Empty)
                throw new ArgumentException("Id must be set.", nameof(this.Id));

            if (string.IsNullOrWhiteSpace(this.Name))
                throw new ArgumentException("Name cannot be empty.", nameof(this.Name));

            if (string.IsNullOrWhiteSpace(this.Semester))
                throw new ArgumentException("Semester cannot be empty.", nameof(this.Semester));

            if (this.Oferts != null && this.Status != null && !Enum.IsDefined(typeof(SubjectStatus), this.Status))
                throw new ArgumentException("Invalid status value.", nameof(this.Status));

            if (this.Oferts == null || !this.Oferts.Any())
                throw new ArgumentException("At least one ofert must be provided.", nameof(this.Oferts));

            if (this.Equivalencies != null && this.Equivalencies.Any(e => string.IsNullOrWhiteSpace(e)))
                throw new ArgumentException("Equivalencies cannot contain empty values.", nameof(this.Equivalencies));

            if (this.Contents != null && this.Contents.Any(c => c is null))
                throw new ArgumentException("Contents cannot contain null values.", nameof(this.Contents));

            if (this.Publishers != null)
            {
                if (this.Publishers.Count > 2)
                    throw new ArgumentException("Publishers cannot contain more than 2 items.", nameof(this.Publishers));

                if (this.Publishers.Count(p => p.IsCurrent == true) > 1)
                    throw new ArgumentException("Only one publisher can be marked as current.", nameof(this.Publishers));

                if (this.Publishers.Count(p => p.IsExpect == true) > 1)
                    throw new ArgumentException("Only one publisher can be marked as expected.", nameof(this.Publishers));
            }
        }
    }
}

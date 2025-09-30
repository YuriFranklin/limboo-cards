namespace LimbooCards.Domain.Entities
{
    using System.Text.Json.Serialization;
    using LimbooCards.Domain.Shared;

    public class Subject
    {
        public Subject(
            Guid? id,
            string? modelId,
            string name,
            string semester,
            SubjectStatus? status,
            List<Ofert> oferts,
            List<string>? equivalencies = null,
            List<Content>? contents = null,
            User? owner = null,
            List<User>? coOwners = null,
            List<SubjectPublisher>? publishers = null
        )
        {
            this.Id = id ?? Guid.CreateVersion7();
            this.ModelId = modelId;
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Semester = semester ?? throw new ArgumentNullException(nameof(semester));
            this.Status = status;
            this.Oferts = oferts ?? new List<Ofert>();
            this.Equivalencies = equivalencies;
            this.Contents = contents;
            this.Owner = owner;
            this.CoOwners = coOwners;
            this.Publishers = publishers;

            Validate();
        }

        [JsonConstructor]
        internal Subject() { }

        [JsonInclude] public Guid Id { get; private set; }
        [JsonInclude] public string? ModelId { get; private set; }
        [JsonInclude] public string Name { get; private set; } = null!;
        [JsonInclude] public User? Owner { get; private set; }
        [JsonInclude] public string Semester { get; private set; } = null!;
        [JsonInclude] public SubjectStatus? Status { get; private set; }
        [JsonInclude] public List<Ofert> Oferts { get; private set; } = new();
        [JsonInclude] public List<string>? Equivalencies { get; private set; }
        [JsonInclude] public List<Content>? Contents { get; private set; }
        [JsonInclude] public List<User>? CoOwners { get; private set; }
        [JsonInclude] public List<SubjectPublisher>? Publishers { get; private set; }

        private void Validate()
        {
            if (Id == Guid.Empty)
                throw new ArgumentException("Id must be set.", nameof(Id));

            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("Name cannot be empty.", nameof(Name));

            if (string.IsNullOrWhiteSpace(Semester))
                throw new ArgumentException("Semester cannot be empty.", nameof(Semester));

            if (Oferts == null || Oferts.Count == 0)
                throw new ArgumentException("At least one ofert must be provided.", nameof(Oferts));

            if (Status != null && !Enum.IsDefined(typeof(SubjectStatus), Status))
                throw new ArgumentException("Invalid status value.", nameof(Status));

            if (Equivalencies != null && Equivalencies.Any(e => string.IsNullOrWhiteSpace(e)))
                throw new ArgumentException("Equivalencies cannot contain empty values.", nameof(Equivalencies));

            if (Contents != null && Contents.Any(c => c is null))
                throw new ArgumentException("Contents cannot contain null values.", nameof(Contents));

            if (Publishers != null)
            {
                if (Publishers.Count > 2)
                    throw new ArgumentException("Publishers cannot contain more than 2 items.", nameof(Publishers));

                if (Publishers.Count(p => p.IsCurrent == true) > 1)
                    throw new ArgumentException("Only one publisher can be marked as current.", nameof(Publishers));

                if (Publishers.Count(p => p.IsExpect == true) > 1)
                    throw new ArgumentException("Only one publisher can be marked as expected.", nameof(Publishers));
            }
        }
    }
}

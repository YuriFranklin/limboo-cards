namespace LimbooCards.Domain.Entities
{
    public class PlannerBucket
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public bool IsDefault { get; private set; }

        public PlannerBucket(Guid id, string name, bool? isDefault = false)
        {
            Id = id;
            Name = name;
            IsDefault = isDefault ?? false;

            Validate();
        }

        private void Validate()
        {
            if (Id == Guid.Empty)
                throw new ArgumentException("PlannerBucket Id cannot be empty.", nameof(Id));

            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("PlannerBucket Name cannot be empty.", nameof(Name));
        }
    }
}

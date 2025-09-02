namespace LimbooCards.Domain.Entities
{
    public class Planner
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public List<PlannerBucket> Buckets { get; private set; }
        public List<PinRule>? PinRules { get; private set; }
        public Planner(Guid id, string name, List<PlannerBucket> buckets, List<PinRule>? pinRules = null)
        {
            this.Id = id;
            this.Name = name;
            this.Buckets = buckets;
            this.PinRules = pinRules;

            Validate();
        }

        private void Validate()
        {
            if (Id == Guid.Empty)
                throw new ArgumentException("Planner Id cannot be empty.", nameof(Id));

            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("Planner name cannot be empty.", nameof(Name));

            if (Buckets == null || Buckets.Count == 0)
                throw new ArgumentException("Planner must contain at least one bucket.", nameof(Buckets));

            if (!Buckets.Any(b => b.IsDefault == true))
                throw new ArgumentException("Planner must contain at least one default bucket.", nameof(Buckets));
        }
    }
}

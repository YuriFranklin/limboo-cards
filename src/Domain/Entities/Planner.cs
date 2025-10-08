namespace LimbooCards.Domain.Entities
{
    public class Planner
    {
        public string Id { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public List<PlannerBucket> Buckets { get; private set; } = new();
        public List<PinRule>? PinRules { get; private set; }
        public Planner(string id, string name, List<PlannerBucket> buckets, List<PinRule>? pinRules = null)
        {
            this.Id = id;
            this.Name = name;
            this.Buckets = buckets;
            this.PinRules = pinRules;

            Validate();
        }

        internal Planner() { }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Id))
                throw new ArgumentException("Planner Id cannot be empty.", nameof(Id));

            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("Planner name cannot be empty.", nameof(Name));

            if (Buckets == null || Buckets.Count == 0)
                throw new ArgumentException("Planner must contain at least one bucket.", nameof(Buckets));

            if (!Buckets.Any(b => b.IsDefault))
                throw new ArgumentException("Planner must contain at least one default bucket.", nameof(Buckets));

            if (!Buckets.Any(b => b.IsEnd))
                throw new ArgumentException("Planner must contain at least one end bucket.", nameof(Buckets));

            if (!Buckets.Any(b => b.IsHistory))
                throw new ArgumentException("Planner must contain at least one history bucket.", nameof(Buckets));
        }
    }
}

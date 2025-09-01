namespace LimbooCards.Domain.Entities
{
    public class Planner
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public List<PlannerBucket> Buckets { get; private set; }

        public Planner(Guid id, string name, List<PlannerBucket>? buckets = null)
        {
            Id = id;
            Name = name;
            Buckets = buckets ?? new List<PlannerBucket>();

            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("Planner name cannot be empty.", nameof(Name));

            if (Buckets == null || Buckets.Count == 0)
                throw new ArgumentException("Planner must contain at least one bucket.", nameof(Buckets));

            if (!Buckets.Any(b => b.IsDefault == true))
                throw new ArgumentException("Planner must contain at least one default bucket.", nameof(Buckets));
        }
    }
}

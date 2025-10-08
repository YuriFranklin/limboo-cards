namespace LimbooCards.Domain.Entities
{
    public class PlannerBucket
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public bool IsDefault { get; private set; }
        public bool IsEnd { get; private set; }
        public bool IsHistory { get; private set; }


        public PlannerBucket(string id, string name, bool isDefault = false, bool isEnd = false, bool isHistory = false)
        {
            Id = id;
            Name = name;
            IsDefault = isDefault;
            IsEnd = isEnd;
            IsHistory = isHistory;

            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Id))
                throw new ArgumentException("PlannerBucket Id cannot be empty.", nameof(Id));

            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("PlannerBucket Name cannot be empty.", nameof(Name));
        }
    }
}

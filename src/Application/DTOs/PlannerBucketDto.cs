namespace LimbooCards.Application.DTOs
{
    public class PlannerBucketDto
    {
        public string Id { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public bool IsDefault { get; private set; }
        public bool IsEnd { get; private set; }
        public bool IsHistory { get; private set; }

    }
}
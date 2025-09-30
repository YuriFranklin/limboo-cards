namespace LimbooCards.Application.DTOs
{
    public class PlannerBucketDto
    {
        public string Id { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public bool IsDefault { get; private set; }
    }
}
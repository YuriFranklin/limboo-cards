namespace LimbooCards.Infra.DTOs
{
    public class PlannerBucketDbDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public bool IsDefault { get; private set; }
    }
}
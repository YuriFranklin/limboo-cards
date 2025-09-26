namespace LimbooCards.Infra.DTOs
{
    public class PlannerDbDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required List<PlannerBucketDbDto> Buckets { get; set; }
        public List<PinRuleDbDto>? PinRules { get; set; }
    }
}
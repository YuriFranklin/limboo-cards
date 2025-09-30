namespace LimbooCards.Application.DTOs
{
    public class CreatePlannerDto
    {
        public string? Id { get; private set; }
        public string Name { get; private set; } = null!;
        public List<PlannerBucketDto> Buckets { get; private set; } = new();
        public List<PinRuleDto>? PinRules { get; private set; }
    }
}
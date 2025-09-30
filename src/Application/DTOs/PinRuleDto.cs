namespace LimbooCards.Application.DTOs
{
    using LimbooCards.Domain.Shared;
    public class PinRuleDto
    {
        public string Expression { get; private set; } = null!;
        public PinColor PinColor { get; private set; }
    }
}
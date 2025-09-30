namespace LimbooCards.Presentation.GraphQL.Models
{
    using LimbooCards.Domain.Shared;
    public class PinRuleModel
    {
        public string Expression { get; private set; } = null!;
        public PinColor PinColor { get; private set; }
    }
}
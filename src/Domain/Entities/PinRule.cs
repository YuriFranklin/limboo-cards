namespace LimbooCards.Domain.Entities
{
    using LimbooCards.Domain.Shared;

    public class PinRule
    {
        public string Expression { get; private set; }
        public PinColor PinColor { get; private set; }
        public PinRule(string expression, PinColor pinColor)
        {
            Expression = expression;
            PinColor = pinColor;

            Validate();
        }
        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Expression))
                throw new ArgumentException("Expression cannot be null or empty.", nameof(Expression));

            if (!Enum.IsDefined(PinColor))
                throw new ArgumentException("Invalid PinColor value.", nameof(PinColor));
        }
    }
}

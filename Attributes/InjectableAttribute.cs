namespace LimbooCards.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class InjectableAttribute : Attribute
    {
        public InjectableAttribute(ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            this.Lifetime = lifetime;
        }

        public ServiceLifetime Lifetime { get; }
    }
}

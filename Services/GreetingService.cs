namespace LimbooCards.Services
{
    using LimbooCards.Attributes;
    using Microsoft.Extensions.DependencyInjection;

    [Injectable(ServiceLifetime.Scoped)]
    public class GreetingService
    {
        public string SayHello()
        {
            return $"Hello, Batata! 👋";
        }
    }
}
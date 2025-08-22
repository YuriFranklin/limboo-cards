namespace LimbooCards.Controllers
{
    using LimbooCards.Services;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class AppController : ControllerBase
    {
        private readonly GreetingService greetingService;

        public AppController(GreetingService greetingService)
        {
            this.greetingService = greetingService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var message = this.greetingService.SayHello();
            return this.Ok(message);
        }
    }
}
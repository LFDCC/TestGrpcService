using Microsoft.AspNetCore.Mvc;

using static TestGrpcNetCoreClient.Greeter;

namespace TestGrpcNetCoreClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly GreeterClient client;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, GreeterClient client)
        {
            _logger = logger;
            this.client = client;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<HelloReply> Get()
        {
            var res = await client.SayHelloAsync(new HelloRequest
            {
                Name = "¿Ó—ﬁ∆‰"
            });
            return res;
        }
    }
}
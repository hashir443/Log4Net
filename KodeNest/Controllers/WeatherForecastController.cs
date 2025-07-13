using Microsoft.AspNetCore.Mvc;

namespace KodeNest.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var response = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            //var connectionString = "Server=DESKTOP-A6COQ9C;Database=AppLog;Integrated Security=True;";
            //var logger = new LogService(connectionString);

            //await logger.SaveLogToDatabaseAsync(new LogPayload
            //{
            //    MethodName = "Startup.Init",
            //    LogMessage = "Application initialized successfully",
            //    ExtraInformation = "Nothing extra",
            //    LogType = LogType.Information,
            //    CreatedBy = 1
            //});

            return response;
        }
    }
}

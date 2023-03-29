using BierBockBackend.Data;
using DataStorage;
using DataStorage.HelpRelations;
using Microsoft.AspNetCore.Mvc;

namespace BierBockBackend.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger, AppDatabaseContext dbAppDatabaseContext)
        {
            _logger = logger;
            //var x = dbAppDatabaseContext.GetDemos();
            //dbAppDatabaseContext.AddDemo(new DemoEntry() { Id = 12, Name = "jona" });

            var x = dbAppDatabaseContext.GetUsers().First();

            var challenge = new Challenge
            {
                Description = "test"
            };

            dbAppDatabaseContext.AddChallenge(challenge);
            dbAppDatabaseContext.AddUser(new User()
            {
                UserChallenges = new[] {new ChallengeUser()
                {
                    Challenge =challenge
                    
                }},
                Name = "Fred",
            });
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
        }
    }
}
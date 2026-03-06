using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;

namespace AzureKeyVaultDemo.Controllers
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
        private readonly IConfiguration configuration;
        //see https://www.youtube.com/watch?v=tfWdGrykfo0&t=1s
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
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

        //basic usage
        [HttpGet("secret")]
        public async Task<IActionResult> GetSecret()
        {
            var secretsClient = new SecretClient(
            new Uri(configuration["AzureKeyVaultUrl"]!),
            new DefaultAzureCredential());//connects locally using Visual Studio or Azure CLI credentials, and in production it will use managed identity if available

            Response<KeyVaultSecret> response = await secretsClient.GetSecretAsync("ApiKey");

            return Ok(response.Value.Value);
        }


        //advanced - using it to fetch connection string
        [HttpGet("connection")]
        public async Task<IActionResult> GetConnection()
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");



            return Ok(connectionString);
        }
    }
}

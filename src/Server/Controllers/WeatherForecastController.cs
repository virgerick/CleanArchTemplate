using CleanArchTemplate.Shared;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace CleanArchTemplate.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IStringLocalizer<WeatherForecastController> _localizer;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IStringLocalizer<WeatherForecastController> localizer)
    {
        _logger = logger;
        _localizer = localizer;
        var test = _localizer["Test"];
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        var request=Request;
        var weathers= Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
            .ToArray();
        foreach (var item in weathers)
        {
            item.Summary = _localizer.GetString(item.Summary!);
        }
        return weathers;
    }
    [HttpPost]
    public async Task Post(WeatherForecast request)
    {

    }
}

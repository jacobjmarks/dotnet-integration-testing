using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Example.Api.Controllers;

[ApiController]
[Route("weather")]
public class WeatherController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public WeatherController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("current-temperature/{latitude}/{longitude}")]
    public async Task<ActionResult<double>> GetCurrentTemperature([FromRoute] double latitude, [FromRoute] double longitude)
    {
        using var client = _httpClientFactory.CreateClient("My External API Client");

        var queryString = new QueryBuilder(new KeyValuePair<string, StringValues>[]
        {
            new("latitude", latitude.ToString()),
            new("longitude", longitude.ToString()),
            new("current_weather", "true"),
        }).ToQueryString();

        using var response = await client.GetAsync("v1/forecast" + queryString);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadFromJsonAsync<JsonElement>();

        var currentTemperature = responseJson
            .GetProperty("current_weather")
            .GetProperty("temperature")
            .GetDouble();

        return currentTemperature;
    }
}

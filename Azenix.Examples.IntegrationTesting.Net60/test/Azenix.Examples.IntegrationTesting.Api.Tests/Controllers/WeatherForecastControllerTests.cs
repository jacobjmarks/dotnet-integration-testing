using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Azenix.Examples.IntegrationTesting.Api.Tests.Controllers;

public class WeatherForecastControllerTests
{
    [Fact]
    public async Task Test()
    {
        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration(config =>
                {

                });
            });

        using var client = factory.CreateClient();
        var response = await client.GetAsync("WeatherForecast");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
    }
}

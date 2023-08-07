using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Api.Tests;

public class Tests
{
    [Fact]
    public async Task GetCurrentTemperature_Returns_200OK()
    {
        //- arrange
        using var testServer = RoutedTestServerBuilder.Build(router =>
        {
            router.MapGet("v1/forecast", async context =>
            {
                await context.Response.WriteAsync(@"{
                    ""latitude"": -27.5,
                    ""longitude"": 153.0,
                    ""generationtime_ms"": 0.1310110092163086,
                    ""utc_offset_seconds"": 0,
                    ""timezone"": ""GMT"",
                    ""timezone_abbreviation"": ""GMT"",
                    ""elevation"": 27.0,
                    ""current_weather"": {
                        ""temperature"": 21.1,
                        ""windspeed"": 16.8,
                        ""winddirection"": 137.0,
                        ""weathercode"": 3,
                        ""is_day"": 1,
                        ""time"": ""2023-08-02T01:00""
                    }
                }");
            });
        });

        using var factory = new CustomWebApplicationFactory()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddHttpClient("My External API Client", _ => testServer.CreateClient())
                        .ConfigurePrimaryHttpMessageHandler(_ => testServer.CreateHandler());
                });
            });

        using var client = factory.CreateClient();

        //- act
        using var response = await client.GetAsync("weather/current-temperature/-27.4679/153.0281");

        //- assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().MatchSnapshot();
    }
}

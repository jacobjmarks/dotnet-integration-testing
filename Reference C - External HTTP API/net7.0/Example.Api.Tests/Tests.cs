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
                await context.Response.WriteAsJsonAsync(new
                {
                    latitude = -27.5,
                    longitude = 153,
                    current_weather = new
                    {
                        temperature = 23.1,
                    },
                });
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

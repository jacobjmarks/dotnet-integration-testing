using System.Net;

namespace Azenix.Examples.IntegrationTesting.Api.Tests.Controllers;

public class WeatherForecastControllerTests
{
    [Fact]
    public async Task Test()
    {
        //- arrange
        using var factory = new MyWebApplicationFactory();
        using var client = factory.CreateClient();

        //- act
        using var response = await client.GetAsync("WeatherForecast");
        var responseContent = await response.Content.ReadAsStringAsync();

        //- assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().NotBeNullOrEmpty();
    }
}

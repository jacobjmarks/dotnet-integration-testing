using System.Net;

namespace Azenix.Examples.IntegrationTesting.Api.Tests;

public class Tests
{
    [Fact]
    public async Task GetEntities_Returns_200OK()
    {
        //- arrange
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        //- act
        using var response = await client.GetAsync("entities");
        var responseContent = await response.Content.ReadAsStringAsync();

        //- assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().MatchSnapshot();
    }
}

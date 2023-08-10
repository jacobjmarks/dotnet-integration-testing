using System.Net;
using System.Security.Claims;

namespace Example.Api.Tests;

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

    [Fact]
    public async Task GetMe_Returns_200OK()
    {
        //- arrange
        using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient()
            .WithClaim(new(ClaimTypes.Name, "John Smith"))
            .WithClaim(new(ClaimTypes.Email, "john.smith@test.account"));

        //- act
        using var response = await client.GetAsync("me");
        var responseContent = await response.Content.ReadAsStringAsync();

        //- assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().MatchSnapshot();
    }
}

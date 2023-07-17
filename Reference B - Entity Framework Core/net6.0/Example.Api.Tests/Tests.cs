using System.Net;
using Example.Api.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Api.Tests;

public class Tests
{
    [Fact]
    public async Task GetEntities_Returns_200OK()
    {
        //- arrange
        var entities = new Entity[]
        {
            new()
            {
                Id = "906f2e82-e6e1-52a4-bb38-6986b258b192",
                Name = "Test Entity 01",
                CreatedAt = DateTimeOffset.Parse("2022-06-18T19:21:50+10:00"),
                LastModifiedAt = DateTimeOffset.Parse("2022-07-08T12:34:37+10:00"),
            },
            new()
            {
                Id = "40c397f2-168e-52c1-8417-5ad4059aca45",
                Name = "Test Entity 02",
                CreatedAt = DateTimeOffset.Parse("2022-02-01T12:32:01+10:00"),
                LastModifiedAt = DateTimeOffset.Parse("2022-04-29T23:58:45+10:00"),
            },
            new()
            {
                Id = "0e038771-4568-54e0-b8c7-16f3d80b4408",
                Name = "Test Entity 03",
                CreatedAt = DateTimeOffset.Parse("2022-03-24T23:51:13+10:00"),
                LastModifiedAt = DateTimeOffset.Parse("2022-06-21T11:55:47+10:00"),
            },
        };

        using var factory = new CustomWebApplicationFactory();

        using (var scope = factory.Services.CreateScope())
        {
            using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Entities.AddRange(entities);
            dbContext.SaveChanges();
        }

        using var client = factory.CreateClient();

        //- act
        using var response = await client.GetAsync("entities");
        var responseContent = await response.Content.ReadAsStringAsync();

        //- assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().MatchSnapshot();
    }
}

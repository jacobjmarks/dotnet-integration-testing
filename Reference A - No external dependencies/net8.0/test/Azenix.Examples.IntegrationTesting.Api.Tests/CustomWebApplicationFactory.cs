using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Azenix.Examples.IntegrationTesting.Api.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        // if necessary, set your desired host environment
        // builder.UseEnvironment("Test");

        // set your required application configuration
        builder.ConfigureHostConfiguration(configurationBuilder =>
        {
            var configuration = new Dictionary<string, string?>
            {
                { "", "" },
            };

            configurationBuilder.AddInMemoryCollection(configuration);
        });

        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // configure your test services
        builder.ConfigureTestServices(services =>
        {
            var mockTimeProvider = new Mock<TimeProvider>();
            mockTimeProvider.Setup(p => p.GetUtcNow()).Returns(DateTimeOffset.Parse("2023-07-06T03:07:00.000Z"));

            services.RemoveAll<TimeProvider>();
            services.AddSingleton(mockTimeProvider.Object);
        });
    }
}
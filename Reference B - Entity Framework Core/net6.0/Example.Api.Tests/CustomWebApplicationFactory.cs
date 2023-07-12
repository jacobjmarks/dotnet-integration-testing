using Example.Api.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Example.Api.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        // if necessary, set your desired host environment
        // builder.UseEnvironment("Test");

        // set your required application configuration
        builder.ConfigureHostConfiguration(configurationBuilder =>
        {
            var configuration = new Dictionary<string, string>
            {
                { "ConnectionStrings:SqlServer", "stub" },
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
            services.Configure<JsonOptions>(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
            });

            var mockTimeProvider = new Mock<ITimeProvider>();
            mockTimeProvider.SetupGet(p => p.UtcNow).Returns(DateTimeOffset.Parse("2023-07-06T03:07:00.000Z"));

            services.RemoveAll<ITimeProvider>();
            services.AddSingleton(mockTimeProvider.Object);

            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            services.AddSingleton(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options);
        });
    }
}

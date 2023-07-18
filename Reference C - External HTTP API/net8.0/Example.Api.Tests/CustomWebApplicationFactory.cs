using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            var configuration = new Dictionary<string, string?>
            {
                { "", "" }
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

            services.AddHttpClient("My External API Client")
                .ConfigurePrimaryHttpMessageHandler(_ => null!);
        });
    }
}
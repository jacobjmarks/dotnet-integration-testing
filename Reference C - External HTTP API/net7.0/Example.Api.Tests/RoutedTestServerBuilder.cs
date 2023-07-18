using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Api.Tests;

public class RoutedTestServerBuilder
{
    private Action<IRouteBuilder> _router = _ => { };

    public RoutedTestServerBuilder WithRouter(Action<IRouteBuilder> router)
    {
        _router = router;
        return this;
    }

    public TestServer Build()
    {
        var builder = new WebHostBuilder();

        builder.ConfigureServices(services =>
        {
            services.AddRouting();
        });

        builder.Configure(app =>
        {
            app.UseRouter(_router);
        });

        return new TestServer(builder);
    }

    public static TestServer Build(Action<IRouteBuilder> router)
    {
        return new RoutedTestServerBuilder()
            .WithRouter(router)
            .Build();
    }
}

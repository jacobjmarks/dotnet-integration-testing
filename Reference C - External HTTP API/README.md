[Home](/) > Reference C - External HTTP API

# Reference C - External HTTP API

This project aims to provide reference for authoring integration tests against a .NET HTTP API which depends on an external/third-party HTTP API.

# Considerations

The following concerns must be catered for when developing integration tests against the system under test.

> Refer to [Reference A](<../Reference A - No external dependencies>) for other solution-agnostic considerations.

## The External API

``` csharp
builder.Services.AddHttpClient("My External API Client");
```

``` csharp
var testServerBuilder = new WebHostBuilder();

testServerBuilder.ConfigureServices(services =>
{
    services.AddRouting();
});

testServerBuilder.Configure(app =>
{
    app.UseRouter(router =>
    {
        // configure your test server endpoints ...
        // for example:
        router.MapGet("v1/hello", context
            => context.Response.WriteAsJsonAsync("world"));
    });
});

using var testServer = new TestServer(testServerBuilder);
```

``` csharp
using var testServer = RoutedTestServerBuilder.Build(router =>
{
    router.MapGet("v1/hello", context
        => context.Response.WriteAsJsonAsync("world"));
});
```

``` csharp
using var factory = new CustomWebApplicationFactory()
    .WithWebHostBuilder(builder =>
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddHttpClient("My External API Client", _ => testServer.CreateClient())
                .ConfigurePrimaryHttpMessageHandler(_ => testServer.CreateHandler());
        });
    });
```

``` csharp
services.AddHttpClient("My External API Client")
    .ConfigurePrimaryHttpMessageHandler(_ => null);
```

# Solutions

## .NET 6

[`./net6.0`](./net6.0)

<!--
## .NET 7

[`./net7.0`](./net7.0)

## .NET 8

[`./net8.0`](./net8.0)
-->

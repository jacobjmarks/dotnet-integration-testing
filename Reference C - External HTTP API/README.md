[Home](/) > Reference C - External HTTP API

# Reference C - External HTTP API

This project aims to provide reference for authoring integration tests against a .NET HTTP API which depends on an external/third-party HTTP API.

# Considerations

The following concerns must be catered for when developing integration tests against the system under test.

> Refer to [Reference A](<../Reference A - No external dependencies>) for other solution-agnostic considerations.

## The External API

It should go without saying that utilising a live, external service during automated testing should not be done; it can introduce fragility and inconsistency in the test suite, add unnecessary strain on the external service, and result in side affects whereby mutations cannot be undone.

We instead look to utilise a fake, in-memory HTTP service which mimics the behaviours of our external API. This can be achieved with a [`TestServer`](https://learn.microsoft.com/en-au/dotnet/api/microsoft.aspnetcore.testhost.testserver?view=aspnetcore-6.0), which is what the `WebApplicationFactory` utilises for communicating with the system under test itself.

For the approach to be effective, all external HTTP API communications should be made via a respective named (or typed) `HttpClient`, registered as below within our system under test:

``` csharp
builder.Services.AddHttpClient("My External API Client");
```

This allows us to replace these clients specifically (via the `WebApplicationFactory`) when faking a particular external API.

Consider the scenario in which we depend on an API which exposes a single `GET /v1/hello` endpoint which returns the textual value `"world"`. We can create a [`TestServer`](https://learn.microsoft.com/en-au/dotnet/api/microsoft.aspnetcore.testhost.testserver?view=aspnetcore-6.0) for this API as follows:

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
        // configure your test server endpoints; for example ...
        router.MapGet("v1/hello", context
            => context.Response.WriteAsJsonAsync("world"));
    });
});

using var testServer = new TestServer(testServerBuilder);
```

This `TestServer` creation can be greatly simplified by introducing some abstraction. For example, a [`RoutedTestServerBuilder`](./net6.0/Example.Api.Tests/RoutedTestServerBuilder.cs):

``` csharp
using var testServer = RoutedTestServerBuilder.Build(router =>
{
    router.MapGet("v1/hello", context
        => context.Response.WriteAsJsonAsync("world"));
});
```

One we have a `TestServer` which fakes our external API, we can utilise it in place of our named `HttpClient` during our test run as below:

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

> Feel free to implement some additional layers of abstraction which make this registration even easier.

You may also find it useful to override the named `HttpClient` by default within your custom `WebApplicationFactory`, as to ensure that the live external API is never utilised if any issues have occurred in the `TestServer` registration or client/handler utilisation. As below:

``` csharp
services.AddHttpClient("My External API Client")
    .ConfigurePrimaryHttpMessageHandler(_ => null);
```

# Solutions

## .NET 6

[`./net6.0`](./net6.0)

## .NET 7

[`./net7.0`](./net7.0)

## .NET 8

[`./net8.0`](./net8.0)

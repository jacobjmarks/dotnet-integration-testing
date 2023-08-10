[Home](/) > Reference A - No external dependencies

# Reference A - No external dependencies

This project aims to provide reference for authoring integration tests against a .NET HTTP API which does not utilise any external dependencies. It should be considered a base template for all other references, containing common components suitable for any solution.

# Considerations

The following concerns must be catered for when developing integration tests against the system under test.

## Authentication

There are many methods of authenticating incoming requests to the application. While it is entirely possible to implement integration tests such that these authentication schemes can be fully exercised &mdash; such as a token provided in the header of a request &mdash; an application may utilise multiple different authentication schemes and it may not be entirely feasible to test them in a black-box production-like manner. Instead, the approach we take is to manipulate the *claims* within our identity and allow them to be set on a per-test and/or per-request basis. This removes the scheme-specific concerns of authenticating a request (these should be tested elsewhere) and allows us to assert the behaviour of our application given a manually-defined set of claims.

To support this, we need a couple of things: a manner in which to define one or more claims as part of a HTTP request; and a custom `AuthenticationHandler` to be used by our `WebApplicationFactory` to utilise the claims defined within the incoming HTTP request instead of any other standard authentication schemes which have been registered.

Respectively, we define a set of extension methods to abstract the transport of claims over HTTP (see [`TestAuthenticationHandlerExtensions`](./net6.0/Example.Api.Tests/TestAuthenticationHandlerExtensions.cs)), and define a [`TestAuthenticationHandler`](./net6.0/Example.Api.Tests/TestAuthenticationHandler.cs) to utilise them. We then utilise this handler over others via a custom [`TestAuthenticationSchemeProvider`](./net6.0/Example.Api.Tests/TestAuthenticationSchemeProvider.cs), registered within our `WebApplicationFactory` as below:

``` csharp
builder.ConfigureTestServices(services =>
{
    services.AddSingleton<IAuthenticationSchemeProvider, TestAuthenticationSchemeProvider>();
});
```

<sup>[Example.Api.Tests/CustomWebApplicationFactory.cs](./net6.0/Example.Api.Tests/CustomWebApplicationFactory.cs)</sup>

All of this allows us to then define claims for requests within our tests to be utilised by the application as below:

``` csharp
using var factory = new CustomWebApplicationFactory();
using var client = factory.CreateClient()
    .WithClaim(new(ClaimTypes.Name, "John Smith"))
    .WithClaim(new(ClaimTypes.Email, "john.smith@test.account"));
```

<sup>[Example.Api.Tests/Tests.cs](./net6.0/Example.Api.Tests/Tests.cs)</sup>

> Note that while the [`Claim`](https://learn.microsoft.com/en-au/dotnet/api/system.security.claims.claim?view=net-6.0) has been used here as the unit with which the developer can set within requests, you can implement an equivalent level of customisability using the same approach at any desired level &mdash; all the way up to the [`AuthenticationTicket`](https://learn.microsoft.com/en-au/dotnet/api/microsoft.aspnetcore.authentication.authenticationticket?view=aspnetcore-6.0) &mdash; depending on your authentication requirements.

## Date/Time Generation

Retrieving the current date time is a common task within applications yet it can add some complexity when authoring any kind of test against the system, as the value will be changing constantly.

The system under test must define a layer of abstraction around this concept to suitably allow tests to override and stabilise the value against change. This is done via a simple [`ITimeProvider`](./net6.0/Example.Api/Services/ITimeProvider.cs) service.

``` csharp
public interface ITimeProvider
{
    DateTimeOffset UtcNow { get; }
}
```

<sup>[Example.Api/Services/ITimeProvider.cs](./net6.0/Example.Api/Services/ITimeProvider.cs)</sup>

This service, registered via Dependency Injection, allows our test framework to use a custom implementation to stabilise the value. As below:

``` csharp
builder.ConfigureTestServices(services =>
{
    var mockTimeProvider = new Mock<ITimeProvider>();
    mockTimeProvider.SetupGet(p => p.UtcNow).Returns(DateTimeOffset.Parse("2023-07-06T03:07:00.000Z"));

    services.RemoveAll<ITimeProvider>();
    services.AddSingleton(mockTimeProvider.Object);
});
```

<sup>[Example.Api.Tests/CustomWebApplicationFactory.cs](./net6.0/Example.Api.Tests/CustomWebApplicationFactory.cs)</sup>

This reference solution sets the value once for all tests, but this behaviour is flexible and the developer can set a specific date/time value for any given test if desired.

# Solutions

## .NET 6

[`./net6.0`](./net6.0)

## .NET 7

[`./net7.0`](./net7.0)

## .NET 8

[`./net8.0`](./net8.0)

### Notable Changes

#### Time Abstraction

[What's new in .NET 8 - Core .NET libraries - Time Abstraction | Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8#time-abstraction)

.NET 8 introduces a new, inbuilt layer of abstraction around the existing static date/time operations, in the form of an abstract `TimeProvider` class. This new class is used in place of the custom date/time abstraction as implemented in both the .NET 6 and .NET 7 solutions.

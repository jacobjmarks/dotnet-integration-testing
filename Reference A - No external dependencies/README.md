[Home](/) > Reference A - No external dependencies

# Reference A - No external dependencies

This project aims to provide reference for authoring integration tests against a .NET HTTP API which does not utilise any external dependencies. It should be considered a base template for all other references, containing common components suitable for any solution.

# Considerations

The following concerns must be catered for when developing integration tests against the system under test.

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
var mockTimeProvider = new Mock<ITimeProvider>();
mockTimeProvider.SetupGet(p => p.UtcNow).Returns(DateTimeOffset.Parse("2023-07-06T03:07:00.000Z"));

services.RemoveAll<ITimeProvider>();
services.AddSingleton(mockTimeProvider.Object);
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

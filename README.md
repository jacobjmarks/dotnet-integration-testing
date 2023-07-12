# Solution Accelerator: .NET Integration Testing

This repository houses a collection of reference solutions with the intent of demonstrating how to author integration tests against a .NET HTTP API which may utilise a range of external dependencies.

Each reference provides an example of how to manage a particular external dependency during integration testing &mdash; discussing recommendations and considerations &mdash; in .NET SDKs 6, 7 and 8 where applicable.

## Approach

### `WebApplicationFactory`

Solutions follow guidance as documented in [Integration tests in ASP.NET Core | Microsoft Learn](https://learn.microsoft.com/en-au/aspnet/core/test/integration-tests). In particular, the utilisation of a custom `WebApplicationFactory` to suitably bootstrap the system under test for integration testing. You'll find the following base utilised throughout all reference solutions:

``` csharp
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        // set your required application configuration
        builder.ConfigureHostConfiguration(configurationBuilder =>
        {
            // ...
        });

        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // configure your test services
        builder.ConfigureTestServices(services =>
        {
            // ...
        });
    }
}
```

> Note that `ConfigureHostConfiguration()` is utilised to set application configuration &mdash; instead of `ConfigureAppConfiguration()` &mdash; as to make this desired configuration visible during application startup. See [dotnet/aspnetcore #37680](https://github.com/dotnet/aspnetcore/issues/37680) and [this comment](https://github.com/dotnet/aspnetcore/issues/37680#issuecomment-1032922656).

## Common Libraries

All solutions utilise the following libraries:

| Library                                                                  | Concern                   | Alternatives                                                                                                      |
| ------------------------------------------------------------------------ | ------------------------- | ----------------------------------------------------------------------------------------------------------------- |
| [xUnit](https://github.com/xunit/xunit)                                  | Test framework            | [NUnit](https://github.com/nunit/nunit), [MSTest](https://github.com/microsoft/testfx)                            |
| [Moq](https://github.com/moq/moq)                                        | Mocking                   | [NSubstitute](https://github.com/nsubstitute/nsubstitute), [FakeItEasy](https://github.com/FakeItEasy/FakeItEasy) |
| [FluentAssertions](https://github.com/fluentassertions/fluentassertions) | Semantic assertion syntax | [Shouldy](https://github.com/shouldly/shouldly)                                                                   |
| [Snapshooter](https://github.com/SwissLife-OSS/snapshooter)              | Snapshot assertions       | [Verify](https://github.com/VerifyTests/Verify)                                                                   |

## Solutions

### [Reference A - No external dependencies](<./Reference A - No external dependencies>)

Integration testing a .NET HTTP API which does not utilise any external dependencies.

This project can be considered a base template for all other reference solutions.

### [Reference B - Entity Framework Core](<./Reference B - Entity Framework Core>)

Integration testing a .NET HTTP API which utilises Entity Framework Core.

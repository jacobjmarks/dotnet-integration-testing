# Solution Accelerator: .NET Integration Testing

This repository houses a collection of reference solutions with the intent of demonstrating how to author integration tests against a .NET HTTP API which may utilise a range of external dependencies.

Each reference provides an example of how to manage a particular external dependency during integration testing &mdash; discussing recommendations and considerations &mdash; in .NET SDKs 6, 7 and 8 where applicable.

Solutions follow guidance as documented in [Integration tests in ASP.NET Core | Microsoft Learn](https://learn.microsoft.com/en-au/aspnet/core/test/integration-tests).

## Common Dependencies

All solutions utilise the following dependencies:

- [xUnit](https://github.com/xunit/xunit)

- [Moq](https://github.com/moq/moq)

- [FluentAssertions](https://github.com/fluentassertions/fluentassertions)

- [Snapshooter](https://github.com/SwissLife-OSS/snapshooter)

## Solutions

### Reference A - No external dependencies

Integration testing a .NET HTTP API which does not utilise any external dependencies.

This project can be considered a base template for all other reference solutions.

[Home](/) > Reference B - Entity Framework Core

# Reference B - Entity Framework Core

This project aims to provide reference for authoring integration tests against a .NET HTTP API which utilises Entity Framework Core.

# Considerations

The following concerns must be catered for when developing integration tests against the system under test.

> Refer to [Reference A](<../Reference A - No external dependencies>) for other solution-agnostic considerations.

## The Database

The system under test will utilise Entity Framework Core to connect to a database of choice. This significant infrastructural dependency must be mocked during our integration tests.

We can do so simply by overriding the `DbContextOptions` service to instead utilise EF Core's in-memory database provider, configured within our test framework as below:

``` csharp
builder.ConfigureTestServices(services =>
{
    services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
    services.AddSingleton(new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options);
});
```

<sup>[Example.Api.Tests/CustomWebApplicationFactory.cs](./net6.0/Example.Api.Tests/CustomWebApplicationFactory.cs)</sup>

Note that there are limitations to be aware of when using the in-memory provider. Please refer to [In-memory as a database fake | Microsoft Learn](https://learn.microsoft.com/en-au/ef/core/testing/choosing-a-testing-strategy#in-memory-as-a-database-fake).

You may also consider mocking the `DbContext`/`DbSet` &mdash; or an internal repository layer which may have been implemented within the system under test &mdash; however, the goal here with these integration tests is to be as black-box as possible, and to push to boundaries of that box as far as possible. That is why we fake the database itself instead of an internal abstraction which is prone to change; asserting the behaviour of the system closer to its production state.

### Further Reading

- [Testing without your production database system - EF Core | Microsoft Learn](https://learn.microsoft.com/en-au/ef/core/testing/testing-without-the-database)

# Solutions

## .NET 6

[`./net6.0`](./net6.0)

## .NET 7

[`./net7.0`](./net7.0)

## .NET 8

[`./net8.0`](./net8.0)

### Notable Changes

#### Time Abstraction

Refer to [Reference A](<../Reference A - No external dependencies>).

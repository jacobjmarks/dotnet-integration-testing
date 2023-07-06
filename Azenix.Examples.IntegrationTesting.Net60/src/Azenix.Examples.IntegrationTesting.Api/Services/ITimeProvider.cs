namespace Azenix.Examples.IntegrationTesting.Api.Services;

public interface ITimeProvider
{
    DateTimeOffset UtcNow { get; }
}

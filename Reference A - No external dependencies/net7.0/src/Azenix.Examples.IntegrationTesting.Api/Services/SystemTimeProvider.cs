namespace Azenix.Examples.IntegrationTesting.Api.Services;

public class SystemTimeProvider : ITimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}

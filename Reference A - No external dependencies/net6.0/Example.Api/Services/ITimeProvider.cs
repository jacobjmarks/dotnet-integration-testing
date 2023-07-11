namespace Example.Api.Services;

public interface ITimeProvider
{
    DateTimeOffset UtcNow { get; }
}

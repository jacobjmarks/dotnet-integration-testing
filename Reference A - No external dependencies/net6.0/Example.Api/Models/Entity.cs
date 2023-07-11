namespace Azenix.Examples.IntegrationTesting.Api.Models;

public record class Entity
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset LastModifiedAt { get; set; }
}

public record class CreateEntityRequestBody
{
    public string Name { get; set; } = null!;
}

public record class UpdateEntityRequestBody
{
    public string Name { get; set; } = null!;
}

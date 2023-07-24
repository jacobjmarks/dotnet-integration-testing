namespace Example.Api.Models;

public record Entity
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset LastModifiedAt { get; set; }
}

public record CreateEntityRequestBody
{
    public string Name { get; set; } = null!;
}

public record UpdateEntityRequestBody
{
    public string Name { get; set; } = null!;
}
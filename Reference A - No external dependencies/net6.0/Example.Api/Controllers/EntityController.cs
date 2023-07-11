using Example.Api.Models;
using Example.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Example.Api.Controllers;

[ApiController]
[Route("entities")]
public class EntityController : ControllerBase
{
    private static readonly Dictionary<string, Entity> _entities = new() {
        { "906f2e82-e6e1-52a4-bb38-6986b258b192", new Entity {
            Id = "906f2e82-e6e1-52a4-bb38-6986b258b192",
            Name = "Test Entity 01",
            CreatedAt = DateTimeOffset.Parse("2022-06-18T19:21:50+10:00"),
            LastModifiedAt = DateTimeOffset.Parse("2022-07-08T12:34:37+10:00"),
        } },
        { "40c397f2-168e-52c1-8417-5ad4059aca45", new Entity {
            Id = "40c397f2-168e-52c1-8417-5ad4059aca45",
            Name = "Test Entity 02",
            CreatedAt = DateTimeOffset.Parse("2022-02-01T12:32:01+10:00"),
            LastModifiedAt = DateTimeOffset.Parse("2022-04-29T23:58:45+10:00"),
        } },
        { "0e038771-4568-54e0-b8c7-16f3d80b4408", new Entity {
            Id = "0e038771-4568-54e0-b8c7-16f3d80b4408",
            Name = "Test Entity 03",
            CreatedAt = DateTimeOffset.Parse("2022-03-24T23:51:13+10:00"),
            LastModifiedAt = DateTimeOffset.Parse("2022-06-21T11:55:47+10:00"),
        } },
    };

    private readonly ITimeProvider _timeProvider;

    public EntityController(ITimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Entity>> GetEntities()
    {
        return _entities.Values;
    }

    [HttpGet("{id}")]
    public ActionResult<Entity> GetEntity([FromRoute] string id)
    {
        return _entities.TryGetValue(id, out var entity) ? entity : NotFound();
    }

    [HttpPost]
    public ActionResult<Entity> CreateEntity([FromBody] CreateEntityRequestBody requestBody)
    {
        var utcNow = _timeProvider.UtcNow;

        var entity = new Entity
        {
            Id = Guid.NewGuid().ToString(),
            Name = requestBody.Name,
            CreatedAt = utcNow,
            LastModifiedAt = utcNow,
        };

        _entities.Add(entity.Id, entity);

        return entity;
    }

    [HttpPut("{id}")]
    public ActionResult<Entity> UpdateBook([FromRoute] string id, [FromBody] UpdateEntityRequestBody requestBody)
    {
        if (!_entities.TryGetValue(id, out var entity))
            return NotFound();

        entity.Name = requestBody.Name;
        entity.LastModifiedAt = _timeProvider.UtcNow;

        return entity;
    }
}
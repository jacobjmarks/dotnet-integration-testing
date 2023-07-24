using Example.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Example.Api.Controllers;

[ApiController]
[Route("entities")]
public class EntityController : ControllerBase
{
    private readonly TimeProvider _timeProvider;
    private readonly ApplicationDbContext _dbContext;

    public EntityController(TimeProvider timeProvider, ApplicationDbContext dbContext)
    {
        _timeProvider = timeProvider;
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Entity>>> GetEntities(CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.Entities
            .AsNoTracking()
            .OrderByDescending(e => e.LastModifiedAt)
            .ToListAsync(cancellationToken);

        return entities;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Entity>> GetEntity([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Entities.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity == null) return NotFound();
        return entity;
    }

    [HttpPost]
    public async Task<ActionResult<Entity>> CreateEntity([FromBody] CreateEntityRequestBody requestBody, CancellationToken cancellationToken = default)
    {
        var utcNow = _timeProvider.GetUtcNow();

        var entity = new Entity
        {
            Id = Guid.NewGuid().ToString(),
            Name = requestBody.Name,
            CreatedAt = utcNow,
            LastModifiedAt = utcNow,
        };

        await _dbContext.Entities.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Entity>> UpdateEntity([FromRoute] string id, [FromBody] UpdateEntityRequestBody requestBody, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Entities.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity == null) return NotFound();

        entity.Name = requestBody.Name;
        entity.LastModifiedAt = _timeProvider.GetUtcNow();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }
}
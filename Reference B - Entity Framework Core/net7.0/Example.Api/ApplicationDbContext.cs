using Example.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Example.Api;

public class ApplicationDbContext : DbContext
{
    public DbSet<Entity> Entities { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
}

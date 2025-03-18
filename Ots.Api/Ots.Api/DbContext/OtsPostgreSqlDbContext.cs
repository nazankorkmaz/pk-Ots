using Microsoft.EntityFrameworkCore;

namespace Ots.Api;

public class OtsPostgreSqlDbContext : DbContext
{
    public OtsPostgreSqlDbContext(DbContextOptions<OtsPostgreSqlDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OtsPostgreSqlDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

}
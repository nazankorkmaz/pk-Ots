using Microsoft.EntityFrameworkCore;

namespace Ots.Api;

public class OtsMsSqlDbContext : DbContext
{
    public OtsMsSqlDbContext(DbContextOptions<OtsMsSqlDbContext> options) : base(options)
    {

    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // load all configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OtsMsSqlDbContext).Assembly);
        // modelBuilder.ApplyConfiguration(new AccountConfiguration());
        base.OnModelCreating(modelBuilder);
    }

}
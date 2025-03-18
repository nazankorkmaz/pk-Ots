using System.Reflection;
using Microsoft.EntityFrameworkCore;
//using Ots.Api.Impl.Cqrs;

namespace Ots.Api;

public class Startup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddSwaggerGen();

        string connectionStringMsSql = Configuration.GetConnectionString("MsSqlConnection");
        string connectionStringPostgresql = Configuration.GetConnectionString("PostgreSqlConnection");

        services.AddDbContext<OtsMsSqlDbContext>(options => { options.UseSqlServer(connectionStringMsSql); });
        services.AddDbContext<OtsPostgreSqlDbContext>(options => { options.UseNpgsql(connectionStringPostgresql); });

        //services.AddMediatR(x=> x.RegisterServicesFromAssemblies(typeof(CreateCustomerCommand).GetTypeInfo().Assembly));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}
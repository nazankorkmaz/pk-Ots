using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Ots.Api.Impl.Cqrs;


using AutoMapper;
using FluentValidation.AspNetCore;
using Ots.Api.Impl.Validation;
using Ots.Api.Mapper;
using Ots.Base;
using Ots.Api.Middleware;


using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


using Ots.Api.Impl.Service;
namespace Ots.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    public static JwtConfig JwtConfig { get; private set; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {

        JwtConfig = Configuration.GetSection("JwtConfig").Get<JwtConfig>();
         services.AddSingleton<JwtConfig>(JwtConfig);


         services.AddControllers().AddFluentValidation(x =>
        {
            x.RegisterValidatorsFromAssemblyContaining<CustomerValidator>();
        });

        services.AddSingleton(new MapperConfiguration(x => x.AddProfile(new MapperConfig())).CreateMapper());

        services.AddControllers();

        services.AddSwaggerGen();

        string connectionStringMsSql = Configuration.GetConnectionString("MsSqlConnection");
        string connectionStringPostgresql = Configuration.GetConnectionString("PostgreSqlConnection");

        services.AddDbContext<OtsMsSqlDbContext>(options => { options.UseSqlServer(connectionStringMsSql); });
        services.AddDbContext<OtsPostgreSqlDbContext>(options => { options.UseNpgsql(connectionStringPostgresql); });

        services.AddMediatR(x=> x.RegisterServicesFromAssemblies(typeof(CreateCustomerCommand).GetTypeInfo().Assembly));

        services.AddScoped<ScopedService>();
        services.AddTransient<TransientService>();
        services.AddSingleton<SingletonService>();

        services.AddScoped<IAccountService, AccountService>();

        services.AddScoped<ITokenService, TokenService>();

         services.AddAuthentication(x =>
         {
             x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
             x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         }).AddJwtBearer(x =>
         {
             x.RequireHttpsMetadata = true;
             x.SaveToken = true;
             x.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuer = true,
                 ValidIssuer = JwtConfig.Issuer,
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtConfig.Secret)),
                 ValidAudience = JwtConfig.Audience,
                 ValidateAudience = false,
                 ValidateLifetime = true,
                 ClockSkew = TimeSpan.FromMinutes(2)
             };
         });
 
         services.AddSwaggerGen(c =>
         {
             c.SwaggerDoc("v1", new OpenApiInfo { Title = "OTS Api Management", Version = "v1.0" });
             var securityScheme = new OpenApiSecurityScheme
             {
                 Name = "Para Management for IT Company",
                 Description = "Enter JWT Bearer token **_only_**",
                 In = ParameterLocation.Header,
                 Type = SecuritySchemeType.Http,
                 Scheme = "bearer",
                 BearerFormat = "JWT",
                 Reference = new OpenApiReference
                 {
                     Id = JwtBearerDefaults.AuthenticationScheme,
                     Type = ReferenceType.SecurityScheme
                 }
             };
             c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
             c.AddSecurityRequirement(new OpenApiSecurityRequirement
             {
                     { securityScheme, new string[] { } }
             });
         });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        
        app.UseMiddleware<HeartBeatMiddleware>();
        app.UseMiddleware<ErrorHandlerMiddleware>();
        app.UseMiddleware<RequestLoggingMiddleware>();


        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        
        app.Use((context, next) =>
        {
            if (!string.IsNullOrEmpty(context.Request.Path) && context.Request.Path.Value.Contains("favicon"))
            {
                return next();
            }
            var singletenService = context.RequestServices.GetRequiredService<SingletonService>();
            var scopedService = context.RequestServices.GetRequiredService<ScopedService>();
            var transientService = context.RequestServices.GetRequiredService<TransientService>();

            singletenService.Counter++;
            scopedService.Counter++;
            transientService.Counter++;

            return next();
        });
        app.Run(async context =>
        {
            var singletenService = context.RequestServices.GetRequiredService<SingletonService>();
            var scopedService = context.RequestServices.GetRequiredService<ScopedService>();
            var transientService = context.RequestServices.GetRequiredService<TransientService>();

            if (!string.IsNullOrEmpty(context.Request.Path) && !context.Request.Path.Value.Contains("favicon"))
            {
                singletenService.Counter++;
                scopedService.Counter++;
                transientService.Counter++;
            }

            await context.Response.WriteAsync($"SingletonService: {singletenService.Counter}\n");
            await context.Response.WriteAsync($"TransientService: {transientService.Counter}\n");
            await context.Response.WriteAsync($"ScopedService: {scopedService.Counter}\n");
        });
    }
}
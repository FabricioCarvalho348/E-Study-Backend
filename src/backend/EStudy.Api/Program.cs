using EStudy.Api;
using EStudy.Api.Common.Api;
using EStudy.Api.Filters;
using EStudy.Api.Token;
using EStudy.Application;
using EStudy.Domain.Security.Tokens;
using EStudy.Infrastructure;
using EStudy.Infrastructure.DataAccess;
using EStudy.Infrastructure.Extensions;
using EStudy.Infrastructure.Migrations;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.AddSecurity();
builder.AddDataContexts();
builder.AddCrossOrigin();
builder.AddDocumentation();
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));
ConfigureServices(builder);
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddHealthChecks().AddDbContextCheck<EStudyDbContext>();

var app = builder.Build();

app.MapHealthChecks("/Health", new HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

app.UseHttpsRedirection();
app.UseRouting();

app.UseSecurity();
app.UseCors(ApiConfiguration.CorsPolicyName);

app.MapControllers();

builder.Services.AddHttpContextAccessor();

if (builder.Configuration.IsTestEnvironment() == false)
{
    app.ConfigureDevEnvironment();
    await MigrateDatabase();
}

app.Run();

async Task MigrateDatabase()
{
    await using var scope = app.Services.CreateAsyncScope();

    await DatabaseMigration.MigrateDatabase(scope.ServiceProvider);
}

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication(builder.Configuration);
}

namespace EStudy.Api
{
    public partial class Program { }
}
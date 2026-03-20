using System.Text;
using EStudy.Api.Token;
using EStudy.Communication;
using EStudy.Domain.Security.Tokens;
using EStudy.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EStudy.Api.Common.Api;

public static class BuilderExtension
{
    public static void AddConfiguration(
        this WebApplicationBuilder builder)
    {
        Configuration.ConnectionString =
            builder
                .Configuration
                .GetConnectionString("DefaultConnection")
            ?? string.Empty;
        Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;
        Configuration.FrontendUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? string.Empty;
    }

    public static void AddDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen(options => { options.CustomSchemaIds(n => n.FullName); });
        builder.Services.AddSwaggerConfig();
    }
    
    public static void AddSecurity(this WebApplicationBuilder builder)
    {
        var signingKey = builder.Configuration.GetValue<string>("Settings:Jwt:SigningKey");

        if (string.IsNullOrWhiteSpace(signingKey))
            throw new InvalidOperationException("JWT signing key is not configured. Set Settings:Jwt:SigningKey in environment variables or user secrets.");

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

        builder.Services.AddAuthorization();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();
    }
    
    public static void AddDataContexts(this WebApplicationBuilder builder)
    {
        builder
            .Services
            .AddDbContext<EStudyDbContext>(
                x => { x.UseNpgsql(Configuration.ConnectionString); });
    }

    public static void AddCrossOrigin(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(
            options => options.AddPolicy(
                ApiConfiguration.CorsPolicyName,
                policy => policy
                    .WithOrigins([
                        Configuration.BackendUrl,
                        Configuration.FrontendUrl
                    ])
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            ));
    }
}



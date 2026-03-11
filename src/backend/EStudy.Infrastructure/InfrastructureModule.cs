using System.Reflection;
using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.User;
using EStudy.Domain.Security.Cryptography;
using EStudy.Domain.Security.Tokens;
using EStudy.Domain.Services.LoggedUser;
using EStudy.Infrastructure.DataAccess;
using EStudy.Infrastructure.DataAccess.Repositories;
using EStudy.Infrastructure.Extensions;
using EStudy.Infrastructure.Security.Cryptography;
using EStudy.Infrastructure.Security.Tokens.Access.Generator;
using EStudy.Infrastructure.Security.Tokens.Access.Validator;
using EStudy.Infrastructure.Services.LoggedUser;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EStudy.Infrastructure;

public static class InfrastructureModule
{ public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddPasswordEncrypter(services, configuration);
        AddRepositories(services);
        AddLoggedUser(services);
        AddTokens(services, configuration);
        
        if (configuration.IsTestEnvironment())
            return;
        
        AddDbContext(services, configuration);
        AddFluentMigratorPostgres(services, configuration);
    }
    
    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        
        services.AddDbContext<EStudyDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseNpgsql(connectionString);
        });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void AddFluentMigratorPostgres(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("EStudy.Infrastructure")).For.All();
        });
    }
    
    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");
        var issuer = configuration.GetValue<string>("Settings:Jwt:Issuer");
        var audience = configuration.GetValue<string>("Settings:Jwt:Audience");
        
        services.AddScoped<IAccessTokenGenerator>(_ => new JwtTokenGenerator(expirationTimeMinutes, signingKey!, issuer!, audience!));
        services.AddScoped<IAccessTokenValidator>(_ => new JwtTokenValidator(signingKey!, issuer!, audience!));
    }
    
    private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();
    
    private static void AddPasswordEncrypter(IServiceCollection services, IConfiguration configuration)
    {
        var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");
        
        services.AddScoped<IPasswordEncrypter>(_ => new Sha512Encrypter(additionalKey!));
    }
}
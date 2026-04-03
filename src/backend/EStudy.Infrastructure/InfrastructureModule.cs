using EStudy.Domain.Repositories;
using EStudy.Domain.Repositories.Event;
using EStudy.Domain.Repositories.Token;
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
using System.Reflection;
using EStudy.Domain.Repositories.UserTask;
using EStudy.Infrastructure.Security.Tokens.Refresh;

namespace EStudy.Infrastructure;

public static class InfrastructureModule
{ public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddPasswordEncrypter(services);
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
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        services.AddScoped<IUserDeleteOnlyRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IUserTaskRepository, UserTaskRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
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

        services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
        services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey!));
        
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
    }
    
    private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();
    
    private static void AddPasswordEncrypter(IServiceCollection services)
    {
        services.AddScoped<IPasswordEncrypter, BCryptNet>();
    }
}
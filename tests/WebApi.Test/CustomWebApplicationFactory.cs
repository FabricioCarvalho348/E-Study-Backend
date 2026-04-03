using CommonTestUtilities.Entities;
using CommonTestUtilities.IdEncryption;
using EStudy.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private EStudy.Domain.Entities.User _user = default!;
    private EStudy.Domain.Entities.RefreshToken _refreshToken = default!;
    private string _password = string.Empty;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<EStudyDbContext>));
                if (descriptor is not null)
                    services.Remove(descriptor);

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
                
                services.AddDbContext<EStudyDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                using var scope = services.BuildServiceProvider().CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<EStudyDbContext>();

                dbContext.Database.EnsureDeleted();

                StartDatabase(dbContext);
            });
    }

    public string GetEmail()
    {
        EnsureSeedDataInitialized();
        return _user.Email;
    }

    public string GetPassword()
    {
        EnsureSeedDataInitialized();
        return _password;
    }

    public string GetName()
    {
        EnsureSeedDataInitialized();
        return _user.Name;
    }

    public string GetRefreshToken()
    {
        EnsureSeedDataInitialized();
        return _refreshToken.Value;
    }

    public Guid GetUserIdentifier()
    {
        EnsureSeedDataInitialized();
        return _user.UserIdentifier;
    }


    private void StartDatabase(EStudyDbContext dbContext)
    {
        (_user, _password) = UserBuilder.Build();
        
        _refreshToken = RefreshTokenBuilder.Build(_user);

        dbContext.Users.Add(_user);
        
        dbContext.RefreshTokens.Add(_refreshToken);

        dbContext.SaveChanges();
    }

    private void EnsureSeedDataInitialized()
    {
        if (_user is not null && _refreshToken is not null && string.IsNullOrWhiteSpace(_password) == false)
            return;

        using var _ = CreateClient();
    }
}
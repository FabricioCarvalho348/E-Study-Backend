using Microsoft.Extensions.Configuration;

namespace EStudy.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static bool IsTestEnvironment(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>("InMemoryTest");
    }
    
    public static string? ConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("DefaultConnection");
    }
}
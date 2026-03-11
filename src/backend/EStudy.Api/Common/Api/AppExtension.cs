using Scalar.AspNetCore;

namespace EStudy.Api.Common.Api;

public static class AppExtension
{
    public static WebApplication ConfigureDevEnvironment(
        this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference("/scalar", options =>
        {
            options
                .WithOpenApiRoutePattern("/swagger/v1/swagger.json");
        });
        app.UseSwaggerSpecification();
        return app;
    }
    
    public static void UseSecurity(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
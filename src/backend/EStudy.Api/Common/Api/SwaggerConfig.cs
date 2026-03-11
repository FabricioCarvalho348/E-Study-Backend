using Microsoft.OpenApi;

namespace EStudy.Api.Common.Api;
public static class SwaggerConfig
{
    private static readonly string AppName = "E-Study API - .NET 10 and Docker";
    private static readonly string AppDescription = $"REST API RESTful developed - {AppName}";

    public static IServiceCollection AddSwaggerConfig(
        this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = AppName,
                Version = "v1",
                Description = AppDescription,
                Contact = new OpenApiContact
                {
                    Name = "Fabricio Carvalho",
                    Email = "fabricio.dev3@gmail.com",
                    
                },
                License = new OpenApiLicense
                {
                    Name = "MIT"
                }
            });

            options.CustomSchemaIds(type => type.FullName);
        });
        return services;
    }

    public static IApplicationBuilder UseSwaggerSpecification(
        this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = "swagger-ui";
            options.DocumentTitle = AppName;
        });
        return app;
    }
}
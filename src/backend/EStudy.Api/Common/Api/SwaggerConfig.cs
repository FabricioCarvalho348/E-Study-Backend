using EStudy.Api.Filters;
using Microsoft.OpenApi;

namespace EStudy.Api.Common.Api;
public static class SwaggerConfig
{
    private static readonly string AppName = "E-Study API - .NET 10 and Docker";
    private static readonly string AppDescription = $"REST API RESTful developed - {AppName}";
    private const string AuthenticationType = "Bearer";

    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = AppName,
                Version = "v1",
                Description = AppDescription,
            });

            options.OperationFilter<IdsFilter>();

            options.AddSecurityDefinition(AuthenticationType, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Autenticação JWT. Digite APENAS o seu token JWT na caixa abaixo (sem a palavra 'Bearer').",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference(AuthenticationType, document),
                    []
                }
            });
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
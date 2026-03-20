using EStudy.Api.Binders;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EStudy.Api.Filters;

public class IdsFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var encryptedIds = context
            .ApiDescription
            .ParameterDescriptions
            .Where(x => x.ModelMetadata.BinderType == typeof(EStudyIdBinder))
            .ToDictionary(d => d.Name, d => d);

        foreach (var parameter in operation.Parameters ?? [])
        {
            if (!string.IsNullOrWhiteSpace(parameter.Name) && encryptedIds.ContainsKey(parameter.Name))
            {
                MakeSchemaString(parameter.Schema);
            }
        }

        foreach (var schema in context.SchemaRepository.Schemas.Values)
        {
            if (schema.Properties is null)
            {
                continue;
            }

            foreach (var property in schema.Properties.ToList())
            {
                if (encryptedIds.ContainsKey(property.Key))
                {
                    schema.Properties[property.Key] = CreateStringSchema(property.Value);
                }
            }
        }
    }

    private static void MakeSchemaString(IOpenApiSchema? schema)
    {
        if (schema is OpenApiSchema mutableSchema)
        {
            mutableSchema.Type = JsonSchemaType.String;
            mutableSchema.Format = null;
        }
    }

    private static IOpenApiSchema CreateStringSchema(IOpenApiSchema? currentSchema)
    {
        if (currentSchema is OpenApiSchema mutableSchema)
        {
            MakeSchemaString(mutableSchema);
            return mutableSchema;
        }

        return new OpenApiSchema
        {
            Type = JsonSchemaType.String,
            Format = null,
        };
    }
}
using Microsoft.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NUlid;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyProject.Api.Extensions;

public class UlidSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(Ulid))
        {
            schema.Type = "string";
            schema.Format = "ulid"; // custom label
            schema.Example = new OpenApiString(Ulid.NewUlid().ToString());
        }
    }
}
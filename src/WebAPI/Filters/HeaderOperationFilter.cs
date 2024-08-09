using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebAPI.Filters
{
    public class HeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            // Add custom headers to all operations
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "request-owner-id",
                In = ParameterLocation.Header,
                Description = "A unique identifier for the request owner.",
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("3cfc7623-773c-4728-a46a-aa51dd1d9706")
                }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "role",
                In = ParameterLocation.Header,
                Description = "Role of the user making the request.",
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("Customer")
                }
            });
        }
    }
}

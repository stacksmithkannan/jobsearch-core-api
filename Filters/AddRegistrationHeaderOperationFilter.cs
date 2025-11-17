using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace JobFinder.API.Filters
{
    public class AddRegistrationHeaderOperationFilter : IOperationFilter
    {
        public void Apply (OpenApiOperation operation,OperationFilterContext context)
        {
            if (context.ApiDescription.RelativePath.Contains("auth/register", StringComparison.OrdinalIgnoreCase))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "X-REG-KEY",
                    In = ParameterLocation.Header,
                    Required = false,
                    Schema = new OpenApiSchema { Type = "string"},
                    Description = "Secret key required for registration"
                });

            }
        }
    }
}

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OpenApi.Swagger
{
#pragma warning disable 1591
	public class AddDeletionIdHeaderOperationFilter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			if (context.ApiDescription.HttpMethod == "DELETE")
			{
				foreach (var openApiResponse in operation.Responses)
				{
					if (!openApiResponse.Value.Headers.ContainsKey("x-deletion-id"))
					{
						// Make sure that the resulting info meets all requirements
						// of the OpenAPI Specification (i.e. has schema or content field set)
						openApiResponse.Value.Headers.Add("x-deletion-id", new OpenApiHeader()
						{
							Required = true,
							Schema = new OpenApiSchema() { Type = "string" },
							Description = "An id identifying the destructive deletion operation in the logs for debugging purposes",
						});
					}
				}
			}
		}
	}
#pragma warning restore 1591
}

using System;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OpenApi.Swagger
{
#pragma warning disable 1591
	public class ApiInfoDocumentFilter : IDocumentFilter
	{
		public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
		{
			if (String.IsNullOrEmpty(swaggerDoc.Info.Description) || !swaggerDoc.Info.Description.Contains("automatically generated"))
			{
				if (!String.IsNullOrEmpty(swaggerDoc.Info.Description))
				{
					swaggerDoc.Info.Description += "</br>";
				}

				swaggerDoc.Info.Description += $"This document was automatically generated as of {DateTime.UtcNow}.";
			}
		}
	}
#pragma warning restore 1591
}

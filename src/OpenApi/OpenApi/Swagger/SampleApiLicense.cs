using System;
using Microsoft.OpenApi.Models;

namespace OpenApi.Swagger
{
#pragma warning disable 1591 // Missing XML Doc
	public class SampleApiLicense : OpenApiLicense
	{
		public SampleApiLicense()
		{
			Name = "Licensed under the Apache 2.0 License";
			Url = new Uri("http://www.apache.org/licenses/LICENSE-2.0.html");
		}
	}
#pragma warning restore 1591
}

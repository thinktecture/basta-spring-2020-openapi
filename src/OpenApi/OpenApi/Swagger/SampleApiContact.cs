using System;
using Microsoft.OpenApi.Models;

namespace OpenApi.Swagger
{
#pragma warning disable 1591 // Missing XML Doc
	public class SampleApiContact : OpenApiContact
	{
		public SampleApiContact()
		{
			Name = "Thinktecture AG - Sebastian Gingter";
			Url = new Uri("https://thinktecture.com");
			Email = "sebastian.gingter@thinktecture.com";
		}
	}
#pragma warning restore 1591
}

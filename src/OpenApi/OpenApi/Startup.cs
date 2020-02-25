using System.Linq;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenApi.Services;

namespace OpenApi
{
#pragma warning disable 1591 // Missing XML Doc
	public class Startup
	{
		private static readonly int[] ApiVersions = new[] { 1, 2, 3, };
		private static readonly int DefaultApiVersion = ApiVersions.Last();


		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<ArticleService>(); // Dummy service for some runtime-persistence

			// Enable Mvc/Web Api Controllers
			services.AddControllers(config =>
			{
				#region Content negotiation
				//config.RespectBrowserAcceptHeader = true;
				//config.InputFormatters.Add(new XmlSerializerInputFormatter(config));
				//config.OutputFormatters.Add(new XmlSerializerOutputFormatter());
				#endregion
			});

			#region AuthN & AuthZ
			services
				.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
				.AddIdentityServerAuthentication(options =>
				{
					options.Authority = "https://demo.identityserver.io";
					options.ApiName = "api";
				});

			services
				.AddAuthorization(config =>
				{
					config.AddPolicy("api", builder =>
					{
						builder.RequireAuthenticatedUser();
						builder.RequireScope("api");
					});
				});
			#endregion

			#region Api Versioning
			services.AddApiVersioning(options =>
			{
				options.DefaultApiVersion = new ApiVersion(DefaultApiVersion, 0);
				options.AssumeDefaultVersionWhenUnspecified = true;
			});

			services.AddVersionedApiExplorer(options =>
			{
				// Swagger uses GroupName to sort endpoints into certain documents, so make sure ApiExplorer
				// sets a group name that corresponds to the version url part
				options.GroupNameFormat = "'v'VVV";

				// Make sure we do not have the {version} part of the Url as parameter in the swagger urls
				options.SubstituteApiVersionInUrl = true;
			});
			#endregion
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			// Activate new endpoint routing introduced in ASP.NET Core 3
			app.UseRouting();

			#region AuthN & AuthZ
			// Order is important: Authorization middleware requires routing information
			// from middleware above, to retrieve required policy information from the
			// (now resolved) controller.action metadata.
			app.UseAuthentication();
			app.UseAuthorization();
			#endregion

			app.UseEndpoints(endpoints =>
			{
				// Dispatch to actual controllers
				endpoints.MapControllers();
			});

			app.UseStaticFiles();
		}
	}
#pragma warning restore 1591
}

using Asp.Versioning.ApiExplorer;

using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace CartService.API.Swagger
{
	/// <summary>
	/// Configures Swagger generation options to support API versioning.
	/// </summary>
	public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
	{
		private readonly IApiVersionDescriptionProvider _provider;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
		/// </summary>
		/// <param name="provider">The API version description provider used to enumerate API versions.</param>
		public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
		{
			_provider = provider;
		}

		/// <summary>
		/// Configures Swagger documentation for each discovered API version.
		/// </summary>
		/// <param name="options">The Swagger generation options to configure.</param>
		public void Configure(SwaggerGenOptions options)
		{
			foreach (var description in _provider.ApiVersionDescriptions)
			{
				options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
			}
		}

		/// <summary>
		/// Creates OpenAPI information for a specific API version.
		/// </summary>
		/// <param name="description">The API version description.</param>
		/// <returns>An <see cref="OpenApiInfo"/> object describing the API version.</returns>
		private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
		{
			var info = new OpenApiInfo
			{
				Title = "Cart Service Web API",
				Version = description.ApiVersion.ToString(),
				Description = "Automatically generated API documentation with versioning support."
			};

			if (description.IsDeprecated)
			{
				info.Description += " This API version has been deprecated.";
			}

			return info;
		}
	}
}

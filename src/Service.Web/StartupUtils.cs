using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.AspNetCore;

namespace Service.Web
{
	public static class StartupUtils
	{
		public static void SetupSwagger(this IApplicationBuilder app, string documentName, string apiName)
		{
			app.UseOpenApi(settings => settings.Path = "/api/v1/{documentName}/swagger/swagger.json");
			app.UseSwaggerUi3(settings =>
			{
				settings.Path = $"/api/v1/{documentName}/swagger";
				settings.SwaggerRoutes.Add(new SwaggerUi3Route("v1", $"/api/v1/{documentName}/swagger/swagger.json"));
				settings.DocumentTitle = $"{apiName} Swagger";
			});
		}

		/// <summary>
		///     Setup swagger ui ba
		/// </summary>
		public static void SetupSwaggerDocumentation(this IServiceCollection services, string documentName, string apiName) => services.AddSwaggerDocument(o =>
		{
			o.Title = $"MyJetEducation {apiName}";
			o.GenerateEnumMappingDescription = true;
			o.DocumentName = documentName;
			o.Version = "v1";
			o.AddSecurity("Bearer", Enumerable.Empty<string>(),
				new OpenApiSecurityScheme
				{
					Type = OpenApiSecuritySchemeType.ApiKey,
					Description = "Bearer Token",
					In = OpenApiSecurityApiKeyLocation.Header,
					Name = "Authorization"
				});
		});

		/// <summary>
		///     Headers settings
		/// </summary>
		public static void ConfigurateHeaders(this IServiceCollection services) =>
			services.Configure<ForwardedHeadersOptions>(options => options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);

		public static void ConfigureJwtBearerOptions(JwtBearerOptions options, string jwtAudience, string jwtSecret)
		{
			options.RequireHttpsMetadata = false;
			options.SaveToken = true;

			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
				ValidateIssuer = false,
				ValidateAudience = true,
				ValidAudience = jwtAudience,
				ValidateLifetime = true,
				LifetimeValidator = (_, expires, _, _) => expires != null && expires > DateTime.UtcNow
			};
		}

		public static void ConfigureAuthenticationOptions(AuthenticationOptions options)
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}
	}
}
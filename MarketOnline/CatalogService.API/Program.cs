using System.Security.Claims;

using CatalogService.Infrastructure.Data;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CatalogService.API
{

	/// <summary>
	/// Program method
	/// </summary>
	public class Program
	{
		/// <summary>
		/// protected constructor
		/// </summary>
		protected Program() { }

		/// <summary>
		/// Main method of the Program
		/// </summary>
		/// <param name="args">args</param>
		public static void Main(string[] args)
		{

			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.Authority = "http://localhost:8080/realms/market-online";
				options.RequireHttpsMetadata = false;
				options.Audience = "market-online";

				options.TokenValidationParameters = new TokenValidationParameters
				{
					NameClaimType = "preferred_username",
					RoleClaimType = ClaimTypes.Role,
					ValidateAudience = true,
					ValidAudience = "market-online",
					ValidateIssuer = true,
					ValidIssuer = "http://localhost:8080/realms/market-online",
				};
			});

			builder.Services.AddAuthorization();

			Bootstrapper.ConfigureContainer(builder);

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(options =>
			{
				options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = Microsoft.OpenApi.Models.ParameterLocation.Header,
					Description = "Use JWT token"
				});

				options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
				{
		{
			new Microsoft.OpenApi.Models.OpenApiSecurityScheme
			{
				Reference = new Microsoft.OpenApi.Models.OpenApiReference
				{
					Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
				});
			});
			builder.Services.AddAutoMapper(typeof(MappingProfile));

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			using (var scope = app.Services.CreateScope())
			{
				var db = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
				db.Database.EnsureCreated();
			}

			app.Run();

		}
	}
}

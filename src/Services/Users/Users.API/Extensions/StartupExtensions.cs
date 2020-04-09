using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

using Users.API.Infrastructure.Repositories;
using Users.API.Application.Models;

namespace Users.API.Extensions
{
	public static class StartupExtensions
	{
		public static IServiceCollection AddCustomJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
		{

			var globalsettings = configuration.GetSection("Global");
			services.Configure<Global>(globalsettings)
				.AddSingleton(sp =>
				sp.GetRequiredService<IOptions<Global>>().Value);


			var secret = configuration.GetSection("Global:Secret").Value;



			services.AddCors();

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})

			.AddJwtBearer(options =>
			{
				options.RequireHttpsMetadata = false;
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
					ValidateIssuer = false,
					ValidateAudience = false,
				};
			});

			return services;
		}
	
		public static IServiceCollection AddUserDatabase(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<UserDatabaseSettings>(
				configuration.GetSection(nameof(UserDatabaseSettings)))

			.AddSingleton<IUserDatabaseSettings>(sp =>
				sp.GetRequiredService<IOptions<UserDatabaseSettings>>().Value)

			.AddSingleton<IUserRepository, UserRepository>();

			return services;
		}
	}
}

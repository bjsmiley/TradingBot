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
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using Hangfire;
using Hangfire.MemoryStorage;

using RabbitMQ.Client;
using TradingBot.Shared.Messaging.Rabbit;
using Microsoft.Extensions.ObjectPool;

namespace TradingBot.Services.StockPrices.API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			services.AddHangfire(x =>
			{
				x.UseMemoryStorage();
			});
			services.AddHangfireServer();

			services.AddRabbitMQ(Configuration);

			
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseHangfireServer();
			app.UseHangfireDashboard();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		
	}

	static class Extensions
	{
		public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration Configuration)
		{
			services.Configure<RabbitMQSettings>(Configuration.GetSection("RabbitMQ"));
			//return services.AddSingleton<IConnection>(x =>
			//{
			//	var settings = x.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
			//	var connectionFactory = new ConnectionFactory();
			//	connectionFactory.Uri = new Uri($"amqp://{settings.UserName}:{settings.Password}@{settings.Host}:{settings.Port}");

			//	return connectionFactory.CreateConnection();

			//});

			services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
			services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicy>();
			services.AddSingleton<IRabbitMQManager, RabbitMQManager>();

			return services;
		}
	}
}

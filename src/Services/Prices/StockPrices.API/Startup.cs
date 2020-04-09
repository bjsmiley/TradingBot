using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StockPrices.API.Application.Models;
using StockPrices.API.Infrastructure.Services;
using StockPrices.API.Infrastructure.Workers;

namespace StockPrices.API
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			var apiKey = Environment.GetEnvironmentVariable("ALPHAVANTAGE_APIKEY") ?? throw new Exception("Enviroment variable 'ALPHAVANTAGE_APIKEY' is not set. This is needed for pricing data.");

			// default alpha vantage client
			services.AddSingleton(new AlphaVantageSettings { ApiKey = apiKey });
			services.AddSingleton<IPricingService, AlphaVantageClient>();


			services.AddSingleton<IRequestPriceService,AlphaVantageWorker>();
			services.AddHostedService<ManagedWorkerService<AlphaVantageWorker>>();
			

			services.AddGrpc();
			
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				//endpoints.MapGrpcService<GreeterService>();
				endpoints.MapGrpcService<PricingService>();

				endpoints.MapGet("/", async context =>
				{
					await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
				});
			});
		}
	}
}

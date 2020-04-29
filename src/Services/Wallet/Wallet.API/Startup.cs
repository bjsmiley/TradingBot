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
using Microsoft.Extensions.Options;
using TradingBot.Services.Wallet.API.Application.Models;
using TradingBot.Services.Wallet.API.Infrastructure.Repositories;

namespace TradingBot.Services.Wallet.API
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
            services.AddLogging(config => config.AddConsole());

            services.AddControllers();


            services.AddHttpsRedirection(opts => opts.HttpsPort = 443);

            // collect wallet db settings from appsettings.json
            services.Configure<WalletDatabaseSettings>(
                Configuration.GetSection(nameof(WalletDatabaseSettings)));

            // inject the settings for use later on
            services.AddSingleton<IWalletDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<WalletDatabaseSettings>>().Value);

            services.AddSingleton<IWalletRepository, WalletRepository>();
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

            //app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

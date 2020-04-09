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

using Wallet.API.Infrastructure.Repositories;
using Wallet.API.Models;

namespace Wallet.API
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

            // securing this api
            //services.Configure<IdentitySettings>(
            //    Configuration.GetSection(nameof(IdentitySettings)));

            //services.AddAuthentication("Bearer")
            //.AddIdentityServerAuthentication(options =>
            //{
            //    options.Authority = Configuration.GetValue<string>("IdentitySettings:ConnectionString");
            //    //options.RequireHttpsMetadata = false;

            //    options.ApiName = "wallet";
            //});

            services.AddHttpsRedirection(opts => opts.HttpsPort = 443);

            // collect wallet db settings from appsettings.json
            services.Configure<WalletDatabaseSettings>(
                Configuration.GetSection(nameof(WalletDatabaseSettings)));

            // inject the settings for use later on
            services.AddSingleton<IWalletDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<WalletDatabaseSettings>>().Value);

            services.AddSingleton<IWalletRepository,WalletRepository>();
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

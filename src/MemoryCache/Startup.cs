using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MemoryCache
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddHttpClient("MercadoBitcoin", client =>
            {
                var apiUrl = Configuration.GetSection("MercadoBitcoin:BaseUrl").Value;
                client.BaseAddress = new Uri(apiUrl);
            });

            services.Configure<CacheSettings>(Configuration.GetSection("Cache"));

            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}

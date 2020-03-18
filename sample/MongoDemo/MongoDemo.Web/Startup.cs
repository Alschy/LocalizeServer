using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alschy.LocalizeServer.Common.Services;
using Alschy.LocalizeServer.Core.Management.Services;
using Alschy.LocalizeServer.MongoDB.Configurations;
using Alschy.LocalizeServer.MongoDB.Mangement.Services;
using Alschy.LocalizeServer.MongoDB.Services;
using Alschy.LocalizeServer.Utils.Caching.Memory.Services;
using Alschy.LocalizeServer.Utils.Caching.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MongoDemo.Web
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
            services.AddSingleton(Configuration.GetSection("MongoDbConnection").Get<MongoDbConnectionConfiguration>());
            services.AddMemoryCache();
            services.AddSingleton<ILocalizeCacheProvider, MemoryLocalizeCacheProvider>();
            services.AddScoped<ILocalizeService, MongoDbLocalizeService>();
            services.AddScoped<ILocalizeManagementService, MongoDbLocalizeManagementService>();
            
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

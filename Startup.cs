using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.MySql.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RegistryPrototype.BE;
using RegistryPrototype.DAL;
using RegistryPrototype.DAL.Repositories;

namespace RegistryPrototype
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton<IRepository<MinimalPackage, string>,PackageRepository>();
            //Take a look at Hangfire, see if just passing an IDBConnection down to the actual middlelayer is an option, with minimal hassle, and if so, use MySQL instead of MMSSQL...
            services.AddHangfire(x => x.UseStorage(new MySqlStorage("server = 192.168.0.18; user id = RegistryClone; password = RegistryClone2019; port = 3306; database = HangfireRegistry;", new MySqlStorageOptions(){TablePrefix = "HangFire"})));
            //services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseHangfireServer();
            app.UseHangfireDashboard();
        }
    }
}

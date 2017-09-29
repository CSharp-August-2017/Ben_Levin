using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
// ADDED BELOW
using MySQL.Data.EntityFrameworkCore.Extensions;
using Microsoft.Extensions.Configuration; //*--Added for secure DB use
using Microsoft.AspNetCore.Hosting;
using user_dash.Models;

namespace user_dash
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            // Add Framework services below + Secure connections
            services.AddDbContext<UDContext>(options => options.UseMySQL(Configuration["DBInfo:ConnectionString"]));
            // Default below
            services.AddMvc();
            services.AddSession();
              
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc();
        }

        //added for secure db connection
        public IConfiguration Configuration { get; private set; } 
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
    }
}

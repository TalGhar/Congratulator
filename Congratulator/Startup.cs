using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Congratulator.Data;
using System;

namespace Congratulator
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration config)
        {
            this.Configuration = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // DB context
            string connectionString = this.Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<UserDbContext>(options => options.UseSqlServer(connectionString));

            // MVC
            services.AddMvc(options => options.EnableEndpointRouting = false);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath);

            this.Configuration = builder.Build();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-apache?view=aspnetcore-7.0
            // https://tutexchange.com/how-to-host-asp-net-core-app-on-ubuntu-with-apache-webserver/
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // Allow the web server to access static file paths in wwwroot folder
            app.UseStaticFiles();

            // Register routes
            app.UseMvc(routes => routes.MapRoute(name: "default", template: "{controller=Users}/{action=Near}/{id?}"));
        }
    }
}
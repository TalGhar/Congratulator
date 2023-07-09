using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Congratulator.Data;
using System;

namespace Congratulator
{
    public class Program
    {
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().Build();

        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<UserDbContext>();

                    context.Database.EnsureCreated();
                }
                catch (InvalidOperationException e)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();

                    logger.LogError(e, "An error occurred creating the DB.");
                }
            }

            host.Run();
        }
    }
}
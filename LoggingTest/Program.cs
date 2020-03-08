using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace LoggingTest
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var host = CreateHostBuilder(args).Build();

      var loggerService = host.Services.GetRequiredService<ILogger<Program>>();

      loggerService.LogError("******************Application built. info************************");

      host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureLogging((loggingBuilder) =>
            {
              loggingBuilder.ClearProviders(); // Clearing Default Error, Debug, EventSource, EventLog mechanisms.
              loggingBuilder.AddConsole();

            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            })
            .UseCustomSerilog();
  }
}

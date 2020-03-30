using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Hosting
{
  public static class SerilogLoggingHelper
  {
    public static IHostBuilder UseCustomSerilog(this IHostBuilder hostBuilder)
    {
      hostBuilder.UseSerilog((hostingContext, loggerConfiguration) =>
      {
        var elasticSearchUrl = hostingContext.Configuration["Logging:ElasticSearchUrl"];
        var elasticIndexFormatRoot = hostingContext.Configuration["Logging:ElasticIndexFormatRoot"];
        var elasticBufferRoot = hostingContext.Configuration["Logging:ElasticBufferRoot"];
        var roolingFileName = hostingContext.Configuration["Logging:RoolingFileName"];

        loggerConfiguration
          .ReadFrom.Configuration(hostingContext.Configuration)
          .Enrich.FromLogContext()
          .WriteTo.Console()
          .WriteTo.File(roolingFileName)
          .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(elasticSearchUrl))
          {
            AutoRegisterTemplate = true,
            AutoRegisterTemplateVersion = Serilog.Sinks.Elasticsearch.AutoRegisterTemplateVersion.ESv6,
            IndexFormat = elasticIndexFormatRoot + "-{0:yyyy.MM.dd}",
            BufferBaseFilename = elasticBufferRoot,
            InlineFields = true,
            //MinimumLogEventLevel = Serilog.Events.LogEventLevel.Warning // This part is configured from appsettings
          }
          );
      });

      return hostBuilder;
    }
}
}

using LogPOC.Logging.Enrichs;
using LogPOC.Logging.Enums;
using LogPOC.Logging.Options;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.Graylog;

namespace LogPOC.Logging.Abstractions
{
    public abstract class BaseLoggerConfiguration
    {
        private readonly LoggerOptions _loggerOptions;

        private readonly IWebHostEnvironment _webHostEnvironment;

        protected BaseLoggerConfiguration(LoggerOptions loggerOptions, IServiceProvider serviceProvider)
        {
            _loggerOptions = loggerOptions;
            _webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
        }

        protected abstract object ConfigureSink();
        protected virtual IConfiguration ReadConfigurationFromAppsetting()
        {
            return new ConfigurationBuilder()
                        .SetBasePath(_webHostEnvironment.ContentRootPath)
                        .AddJsonFile($"Configurations/{_loggerOptions.LogFileName}.json")
                        .AddJsonFile($"Configurations/{_loggerOptions.LogFileName}{_webHostEnvironment.EnvironmentName}.json", optional: true)
                        .Build();
        }

        protected virtual LoggerConfiguration LogConfiguration()
        {
            IConfiguration configuration = ReadConfigurationFromAppsetting();

            LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.With(new DateTimeEnricher())
                    .Enrich.WithProperty("Environment", _webHostEnvironment.EnvironmentName)
                    .Enrich.WithProperty("Application", _webHostEnvironment.ApplicationName)
                    .Enrich.WithExceptionDetails()
                    .ReadFrom.Configuration(configuration);

            loggerConfiguration = _loggerOptions.LogType switch
            {
                LogType.GrayLog => loggerConfiguration.WriteTo.Graylog((GraylogSinkOptions)ConfigureSink()),
                LogType.ELK => loggerConfiguration.WriteTo.Elasticsearch((ElasticsearchSinkOptions)ConfigureSink()),
                _ => throw new NotImplementedException("Log tipini implemente etmedin kötü")
            };

            if (_loggerOptions.WriteDebug)
            {
                loggerConfiguration = loggerConfiguration.WriteTo.Debug();
            }

            if (_loggerOptions.WriteConsole)
            {
                loggerConfiguration = loggerConfiguration.WriteTo.Console();
            }

            return loggerConfiguration;
        }
    }
}

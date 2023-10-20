using LogPOC.Logging.Abstractions;
using LogPOC.Logging.Options;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace LogPOC.Logging.Configurations
{
    public class ELKConfiguration : BaseLoggerConfiguration
    {
        private readonly LoggerConfiguration LoggerConfiguration;
        private readonly LoggerOptions _loggerOptions;
        private readonly string _environmentName;

        public ELKConfiguration(IServiceProvider serviceProvider, LoggerOptions loggerOptions):base(loggerOptions, serviceProvider)
        {
            IWebHostEnvironment webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
            _environmentName = webHostEnvironment.EnvironmentName;

            _loggerOptions = loggerOptions;
            LoggerConfiguration = LogConfiguration();
        }

        protected override object ConfigureSink()
        {
            string applicationName = Assembly.GetEntryAssembly()?.GetName().Name?.ToLowerInvariant().Replace(".", "-");
            Uri elasticSearchAddress = new(_loggerOptions.ELKOptions.Address);
            return new ElasticsearchSinkOptions(elasticSearchAddress)
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{applicationName}-{_environmentName}-{DateTime.UtcNow:yyyy-MM}"
            };
        }

        public LoggerConfiguration GetLoggerConfiguration()
        {
            return LoggerConfiguration;
        }
    }
}

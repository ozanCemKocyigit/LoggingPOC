using LogPOC.Logging.Abstractions;
using LogPOC.Logging.Options;
using Serilog;
using Serilog.Sinks.Graylog;

namespace LogPOC.Logging.Configurations
{
    public sealed class GraylogConfiguration : BaseLoggerConfiguration
    {
        private readonly LoggerConfiguration LoggerConfiguration;

        public GraylogConfiguration(IServiceProvider serviceProvider, LoggerOptions options) : base(options, serviceProvider)
        {
            LoggerConfiguration = LogConfiguration();
        }

        public LoggerConfiguration GetLoggerConfiguration()
        {
            return LoggerConfiguration;
        }

        protected override object ConfigureSink()
        {
            return new GraylogSinkOptions();
        }
    }
}

using LogPOC.Logging.Configurations;
using LogPOC.Logging.Enums;
using LogPOC.Logging.Options;
using Serilog;

namespace LogPOC.Logging.Factory
{
    public static class LoggerCreator
    {
        public static LoggerConfiguration Create(IServiceProvider serviceProvider)
        {
            LoggerOptions loggerOptions = serviceProvider.GetRequiredService<IConfiguration>().GetSection("Logger").Get<LoggerOptions>();

            return loggerOptions.LogType switch
            {
                LogType.None => new LoggerConfiguration(),
                LogType.GrayLog => new GraylogConfiguration(serviceProvider ,loggerOptions).GetLoggerConfiguration(),
                LogType.ELK => new ELKConfiguration(serviceProvider, loggerOptions).GetLoggerConfiguration(),
                _=> throw new NotImplementedException("yine bir şeyi yanlış yaptın kötü")
            };
        }
    }
}

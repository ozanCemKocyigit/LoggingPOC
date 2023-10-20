using LogPOC.Logging.Enums;

namespace LogPOC.Logging.Options
{
    public class LoggerOptions
    {
        public LogType LogType { get; set; } = LogType.ELK;
        public bool WriteConsole { get; set; } = false;
        public bool WriteDebug { get; set; } = false;
        public string LogFileName
        {
            get
            {
                return LogType switch
                {
                    LogType.GrayLog => Constants.LoggingConstants.GrayLogFileName,
                    LogType.ELK => Constants.LoggingConstants.ELKFileName,
                    _ => string.Empty
                };
            }
        }

        public GrayLogOptions GrayLogOptions { get; set; } = new();
        public ELKOptions ELKOptions { get; set; } = new();
    }
}

using Serilog.Core;
using Serilog.Events;

namespace LogPOC.Logging.Enrichs
{
    internal class DateTimeEnricher : ILogEventEnricher
    {
        private const string DateTimeFormat = "dd-MM-yyyy HH:mm:ss.fff";
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(new LogEventProperty("timestamp", new ScalarValue(DateTime.Now.ToString(DateTimeFormat))));
        }
    }
}

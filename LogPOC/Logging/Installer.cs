using LogPOC.Logging.Factory;
using Serilog;

namespace LogPOC.Logging
{
    public static class Installer
    {
        public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
        {
            using ServiceProvider serviceProvider = builder.Services.BuildServiceProvider();

            LoggerConfiguration loggerConfiguration = LoggerCreator.Create(serviceProvider);

            builder.Host.UseSerilog(loggerConfiguration.CreateLogger());

            return builder;

        }

        public static IApplicationBuilder UseSerilog(this WebApplication app)
        {
            ILoggerFactory loggerFactory = app.Services.GetService<ILoggerFactory>();

            loggerFactory?.AddSerilog();

            return app;
        }
    }
}

using Serilog;
using Serilog.Events;

namespace WorkVista.API.ExtensionClasses
{
    public static class HostExtension
    {
        public static IHostBuilder ConfigureSeriLog(this IHostBuilder host)
        {
            return host.UseSerilog((ctx, lc) =>
            {
                // Log to Rolling File
                lc.WriteTo.File("Logs/InfoLog-.txt",
                  rollingInterval: RollingInterval.Day,
                  restrictedToMinimumLevel: LogEventLevel.Information);
                lc.WriteTo.File("Logs/ErrorLog-.txt",
                  rollingInterval: RollingInterval.Day,
                  restrictedToMinimumLevel: LogEventLevel.Error);
                lc.WriteTo.File("Logs/WarningLog-.txt",
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Warning);
            });
        }
    }

}

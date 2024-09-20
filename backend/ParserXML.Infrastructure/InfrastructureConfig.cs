using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Web;
using ParserXML.Core.Providers;
using ParserXML.Core.Repositories;
using ParserXML.Infrastructure.Repositories;
using System.Diagnostics;
using LogLevel = NLog.LogLevel;

namespace ParserXML.Infrastructure
{
    public static class InfrastructureConfig
    {
        public static void SetupWithoutDb(this IServiceCollection services)
        {
            AddProviderServices(services); 

            AddNLog();
        }
        private static void AddProviderServices(this IServiceCollection services)
        {
            services.AddScoped<IExcelService, ExcelServices>();
            services.AddScoped<ITableDatasRepository, TableDataRepositories>(); 
        }

        private static void AddNLog()
        {
            LogManager.Setup().SetupExtensions(s => s.RegisterAssembly("NLog.Web.AspNetCore"));

            var nlogConfig = new LoggingConfiguration();
            var consoleTarget = new ConsoleTarget("console");
            var debuggerTarget = new DebuggerTarget("debugger");

            nlogConfig.AddTarget(consoleTarget);
            nlogConfig.AddTarget(debuggerTarget);

            nlogConfig.AddRule(LogLevel.Debug, LogLevel.Fatal, consoleTarget);
            nlogConfig.AddRule(LogLevel.Trace, LogLevel.Fatal, debuggerTarget);

            LogManager.Configuration = nlogConfig;
            LogManager.ThrowConfigExceptions = true;
        }
    }
}

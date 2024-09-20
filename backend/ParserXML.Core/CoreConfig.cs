using Microsoft.Extensions.DependencyInjection;
using ParserXML.Core.Providers;
using ParserXML.Core.Repositories;
using ParserXML.Core.Services.Implementations;
using ParserXML.Core.Services.Interfaces;

namespace ParserXML.Core
{
    public static class CoreConfig
    {
        public static void Setup(this IServiceCollection services)
        {
            AddBusinessServices(services)
              .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
           services.AddScoped<ITableDatasService, TableDatasService>();

            return services;
        }
    }
}

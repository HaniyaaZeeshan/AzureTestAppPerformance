using AzureTestAppPerformance.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AzureTestAppPerformance.Startup))]

namespace AzureTestAppPerformance
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Register services for Dependency Injection (DI)
            builder.Services.AddSingleton<ISalesOrderService, SalesOrderService>();
        }
    }
}

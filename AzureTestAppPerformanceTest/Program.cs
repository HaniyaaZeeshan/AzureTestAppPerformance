using AzureTestAppPerformanceTest.config;
using AzureTestAppPerformanceTest.Services;
using AzureTestAppPerformanceTest.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using VRI.Integrations.Connectors.Interfaces;
using VRI.Integrations.Connectors.Services;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        services.AddSingleton<IConfiguration>(configuration);

        services.Configure<ShopifyConfiguration>(configuration.GetSection("ShopifyConfiguration"));
        services.AddSingleton<IShopifyConfiguration>(sp =>
            sp.GetRequiredService<IOptions<ShopifyConfiguration>>().Value);

        services.AddHttpClient(); // Register HttpClient

        services.AddSingleton<IHttpService, HttpService>();
        services.AddSingleton<IGenericTokenClient, GenericTokenClient>();
        services.AddSingleton<PerformanceTester>();
        services.AddSingleton<AppConfig>();
    });

var host = builder.Build();

try
{
    var performanceTester = host.Services.GetRequiredService<PerformanceTester>();
    await performanceTester.RunPerformanceTests();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("❌ An error occurred during performance test execution:");
    Console.WriteLine(ex.ToString());
    Console.ResetColor();
}

Console.WriteLine("✅ Performance test execution complete.");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

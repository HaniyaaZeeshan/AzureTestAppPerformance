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
        // Step 1: Build Configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Set working directory
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Read appsettings.json
            .Build();

        services.AddSingleton<IConfiguration>(configuration);

        // Step 2: Register services
        services.Configure<ShopifyConfiguration>(configuration.GetSection("ShopifyConfiguration"));
        services.AddSingleton<IShopifyConfiguration>(sp =>
            sp.GetRequiredService<IOptions<ShopifyConfiguration>>().Value);

        services.AddHttpClient(); // Register HttpClient

        // Register other services
        services.AddSingleton<IHttpService, HttpService>();
        services.AddSingleton<IGenericTokenClient, GenericTokenClient>();
        services.AddSingleton<PerformanceTester>();

        // Additional registrations
        services.AddSingleton<AppConfig>();
    });

var host = builder.Build();

// Step 3: Resolve and run
var performanceTester = host.Services.GetRequiredService<PerformanceTester>();
await performanceTester.RunPerformanceTests();




using AzureTestAppPerformanceTest.config;
using AzureTestAppPerformanceTest.Models;
using AzureTestAppPerformanceTest.Services;
using AzureTestAppPerformanceTest.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NBomber.CSharp;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AzureTestAppPerformanceTest
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            // Register services
            serviceCollection.AddSingleton<IShopifyConfiguration>();
            serviceCollection.AddSingleton<AppConfig>(); // Load app config
            serviceCollection.AddSingleton<HttpClient>();
            serviceCollection.AddSingleton<PerformanceTestConfig>(); // Add the PerformanceTestConfig
            serviceCollection.AddSingleton<IHttpService, HttpService>();
            serviceCollection.AddSingleton<PerformanceTester>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Resolve PerformanceTester and Run Tests
            var performanceTester = serviceProvider.GetRequiredService<PerformanceTester>();
            performanceTester.RunPerformanceTests();

            // Keep Console Open
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
          
        }
    }
}

using AzureTestAppPerformanceTest.config;
using AzureTestAppPerformanceTest.Dto;
using AzureTestAppPerformanceTest.Models;
using AzureTestAppPerformanceTest.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NBomber.Contracts.Stats;
using NBomber.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTestAppPerformanceTest.Tests
{
    public class PerformanceTester
    {
        private readonly IHttpService _httpService;
        private readonly AppConfig _config;
        private readonly PerformanceTestSettings _testSettings;

        public PerformanceTester(IHttpService httpService, AppConfig config)
        {
            _httpService = httpService;
            _config = config;

            // Load performance test settings
            var configLoader = new PerformanceTestConfig();
            _testSettings = configLoader.GetSettings();
        }

        public void RunPerformanceTests()
        {
            Console.WriteLine("🚀 Running NBomber Performance Tests...");

            // 1. GET Sales Orders - Constant Load
            var constantLoadScenario = Scenario.Create("GET Sales Orders - Constant Load", async context =>
            {
                var salesOrders = await _httpService.GetListAsync<SalesOrder>(_config.GetSalesOrderEndpoint);

                return salesOrders != null && salesOrders.Any() ? Response.Ok() : Response.Fail();
            })
            .WithWarmUpDuration(TimeSpan.FromSeconds(_testSettings.WarmUpDurationInSeconds)) // Set warm-up duration
            .WithLoadSimulations(Simulation.KeepConstant(copies: _testSettings.LoadSimulation.UserCount, TimeSpan.FromSeconds(_testSettings.LoadSimulation.DurationInSeconds))); // Constant user load

            // 2. GET Sales Orders - Spike Load (using Inject)
            var spikeLoadScenario = Scenario.Create("GET Sales Orders - Spike Load", async context =>
            {
                var salesOrders = await _httpService.GetListAsync<SalesOrder>(_config.GetSalesOrderEndpoint);

                return salesOrders != null && salesOrders.Any() ? Response.Ok() : Response.Fail();
            })
            .WithWarmUpDuration(TimeSpan.FromSeconds(_testSettings.WarmUpDurationInSeconds)) // Set warm-up duration
            .WithLoadSimulations(
                Simulation.Inject(_testSettings.LoadSimulation.UserCount, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30)) // Spike load over 30 seconds
            );

            // 3. GET Sales Orders - Ramping Load
            var rampingLoadScenario = Scenario.Create("GET Sales Orders - Ramping Load", async context =>
            {
                var salesOrders = await _httpService.GetListAsync<SalesOrder>(_config.GetSalesOrderEndpoint);

                return salesOrders != null && salesOrders.Any() ? Response.Ok() : Response.Fail();
            })
            .WithWarmUpDuration(TimeSpan.FromSeconds(_testSettings.WarmUpDurationInSeconds)) // Set warm-up duration
            .WithLoadSimulations(
                Simulation.RampingInject(_testSettings.LoadSimulation.UserCount, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30)) // Ramping load over 30 seconds
            );

            // Register all three scenarios and run them simultaneously (in parallel by default)
            var result = NBomberRunner
                .RegisterScenarios(constantLoadScenario, spikeLoadScenario, rampingLoadScenario)
                .WithReportFolder("Reports")                     
                .WithReportFileName("my_test_report")            
                .WithReportFormats(ReportFormat.Html, ReportFormat.Csv, ReportFormat.Md) 
                .Run();

            PrintResults(result);
        }

       


        private void PrintResults(NodeStats stats)
        {
            Console.WriteLine("\n📊 NBomber Test Results:");
            Console.WriteLine($"Total Requests: {stats.AllRequestCount}");
            Console.WriteLine($"Successful Requests: {stats.AllOkCount}");
            Console.WriteLine($"Failed Requests: {stats.AllFailCount}");
            Console.WriteLine($"Test completed. Success rate: {stats.AllOkCount}/{stats.AllRequestCount}");
            Console.WriteLine($"Test Duration: {stats.Duration}");

            double errorRate = (double)stats.AllFailCount / stats.AllRequestCount * 100;
            Console.WriteLine($"Error Rate: {errorRate}%");


        }
    }
}

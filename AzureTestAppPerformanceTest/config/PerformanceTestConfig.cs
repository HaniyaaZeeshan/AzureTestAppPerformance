using AzureTestAppPerformanceTest.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTestAppPerformanceTest.config
{
    public class PerformanceTestConfig
    {
        private readonly IConfiguration _configuration;

        public PerformanceTestConfig()
        {
            // Get the environment from the environment variable (defaults to "development")
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "development";

            // Load the configuration file based on environment
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Set the base directory for file searching
                .AddJsonFile($"performanceTestSettings.{environment}.json", optional: false, reloadOnChange: true) // Load the correct config file
                .Build();
        }

        // Get the loaded configuration as a strongly typed object
        public PerformanceTestSettings GetSettings()
        {
            var settings = _configuration.GetSection("performanceTestSettings").Get<PerformanceTestSettings>();
            return settings;
        }
    }
}

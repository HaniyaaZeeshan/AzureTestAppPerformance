using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTestAppPerformanceTest.config
{
    public class AppConfig
    {
        private readonly IConfiguration _configuration;

        public AppConfig()
        {
            // Load the configuration file
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "development";
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true)
                .Build();
        }

        // Expose the full endpoint as a property
        public string GetSalesOrderEndpoint => $"{_configuration["baseURL"]}{_configuration["GetSalesOrderEndpoint"]}";
        public string GetD365EndPoint => $"{_configuration["D365BaseUrl"]}{_configuration["D365Endpoint"]}";
    }
}

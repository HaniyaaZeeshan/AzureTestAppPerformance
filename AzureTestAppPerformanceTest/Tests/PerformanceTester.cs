using AzureTestAppPerformanceTest.config;
using AzureTestAppPerformanceTest.Common;
using AzureTestAppPerformanceTest.Dto;
using AzureTestAppPerformanceTest.Exceptions;
using AzureTestAppPerformance.Model;
using AzureTestAppPerformanceTest.Services;
using NBomber.Contracts.Stats;
using NBomber.CSharp;
using VRI.Integrations.Connectors.Interfaces;
using VRI.Integrations.Connectors.Models;
using AzureTestAppPerformance.Services;
using SalesOrderRequest = AzureTestAppPerformance.Model.SalesOrderRequest;
using SalesHeader = AzureTestAppPerformance.Model.SalesHeader;
using SalesLine = AzureTestAppPerformance.Model.SalesLine;
using MarkupTrans = AzureTestAppPerformance.Model.MarkupTrans;
using Payment = AzureTestAppPerformance.Model.Payment;
using AzureTestAppPerformanceTest.Models;

namespace AzureTestAppPerformanceTest.Tests
{
    public class PerformanceTester
    {
        private readonly IHttpService _httpService;
        private readonly AppConfig _config;
        private readonly PerformanceTestSettings _testSettings;
        private readonly IGenericTokenClient _genericTokenClient;
        private readonly IShopifyConfiguration _shopifyConfiguration;

        public PerformanceTester(IHttpService httpService, AppConfig config,
                                  IGenericTokenClient genericTokenClient,
                                  IShopifyConfiguration shopifyConfiguration)
        {
            _httpService = httpService;
            _config = config;
            _genericTokenClient = genericTokenClient;
            _shopifyConfiguration = shopifyConfiguration;

            // Load performance test settings
            var configLoader = new PerformanceTestConfig();
            _testSettings = configLoader.GetSettings();
        }

        public async Task RunPerformanceTests()
        {
            Console.WriteLine("🚀 Running NBomber Performance Tests...");
            var token = await GetTokenAsync();  // <-- now inside same class
            Console.WriteLine($"🔑 Token Generated: {token}");
            // Add token to HttpClient headers
            await _httpService.AddAuthorizationHeader(token);
            await _httpService.AddSubscriptionKeyHeader(_shopifyConfiguration.SubscriptionId);
            // Create the payload for the POST request
            var salesOrderRequest = new SalesOrderRequest
            {
                CurrentCompany = "USV1",
                SalesHeadersList = new List<SalesHeader>
        {
            new SalesHeader
            {
                ExternalOrderId = "6024983511263_test",
                SalesOrderPoolId = "RdyInv",
                CurrencyCode = "USD",
                OrderClassificationId = "Standalone",
                InvoiceCustomerAccountNumber = "C-000064",
                CustomersOrderReference = "#DEV1120048",
                DeliveryAddressCity = "New York",
                DeliveryAddressCountryRegionId = "USA",
                DeliveryAddressDescription = "John Luna",
                DeliveryAddressName = "John Luna",
                DeliveryAddressStateId = "NY",
                DeliveryAddressStreet = "358 3rd Ave",
                DeliveryAddressZipCode = "10016-9004",
                Email = "zunaira.javed@vuori.com",
                OrderDate = DateTime.Parse("3/13/2025 4:59:15 PM"),
                OrderingCustomerAccountNumber = "C-000064",
                Phone = "19072622578",
                RequestedShippingDate = DateTime.Parse("3/13/2025 4:59:15 PM"),
                SalesOrderOriginCode = "Ecommerce",
                ModeOfDelivery = "U07",
                OrderResponsiblePersonnelNumber = "19072622578",
                OrderTakePersonnelNumber = "19072622578",
                SalesType = "Sales",
                MarkupTransList = new List<MarkupTrans>
                {
                    new MarkupTrans
                    {
                        MarkupCode = "SHIPPING",
                        MarkupValue = 15.0m,
                        TransTxt = "SHIPPING"
                    }
                },
                PaymentList = new List<Payment>
                {
                    new Payment
                    {
                        PaymentAmount = 75.00m,
                        PaymentType = "10",
                        PaymentReference = "7196183789791",
                        PaymentKind = "SALE"
                    },
                    new Payment
                    {
                        PaymentAmount = 14.00m,
                        PaymentType = "10",
                        PaymentReference = "7196183822559",
                        PaymentKind = "SALE"
                    }
                },
                SalesLinesList = new List<SalesLine>
                {
                    new SalesLine
                    {
                        ExternalLineId = "14786851569887",
                        ItemNumber = "V1000HBKSML",
                        ItemBarCode = "196304023835",
                        LineDescription = "Zephyr Polo | Black Heather - Black Heather / S",
                        OrderedSalesQuantity = 1,
                        SalesPrice = 74.0m,
                        SalesUnitSymbol = "EA",
                        DeliveryModeCode = "U07",
                        IsPayloadPrice = true,
                        SalesTaxItemGroupCode = "NT",
                        LineDiscountAmount = 0,
                        TaxAmount = 0.00m,
                        TaxCode = "NO-Tax"
                    }
                }
            }
        }
            };

            // 1. GET Sales Orders - Constant Load
            //var constantLoadScenario = Scenario.Create("GET Sales Orders - Constant Load", async context =>
            //{
            //    var salesOrders = await _httpService.PostAsync<SalesOrder>(_config.GetD365EndPoint);

            //    return salesOrders != null && salesOrders.Any() ? Response.Ok() : Response.Fail();
            //})
            //.WithWarmUpDuration(TimeSpan.FromSeconds(_testSettings.WarmUpDurationInSeconds)) // Set warm-up duration
            //.WithLoadSimulations(Simulation.KeepConstant(copies: _testSettings.LoadSimulation.UserCount, TimeSpan.FromSeconds(_testSettings.LoadSimulation.DurationInSeconds))); // Constant user load

            // Create the payload for the POST request
            // Generate the SalesOrderRequest using Bogus
            //var salesOrderRequestService = new SalesOrderService();
            //var salesOrderRequest = await salesOrderRequestService.GetSalesOrderRequestAsync();
            var wrapper = new SalesOrderRequestWrapper
            {
                SalesOrders = salesOrderRequest
            };


            // 1. GET Sales Orders - Constant Load
            var constantLoadScenario = Scenario.Create("GET Sales Orders - Constant Load", async context =>
            {
                // Send the POST request with the SalesOrderRequest payload
                var response = await _httpService.PostAsync<SalesOrderRequestWrapper, ShopifyApiResponse>(_config.GetD365EndPoint, wrapper, token, _shopifyConfiguration.SubscriptionId);
                Console.WriteLine($"Response: Success = {response?.Success}, Message = {response?.Message}");

                return response != null && response.Success ? Response.Ok() : Response.Fail();
            })
            .WithWarmUpDuration(TimeSpan.FromSeconds(_testSettings.WarmUpDurationInSeconds)) // Set warm-up duration
            .WithLoadSimulations(Simulation.KeepConstant(copies: _testSettings.LoadSimulation.UserCount, TimeSpan.FromSeconds(_testSettings.LoadSimulation.DurationInSeconds))); // Constant user load


            // Register scenarios and run
            var result = NBomberRunner
                .RegisterScenarios(constantLoadScenario)
                .WithReportFolder("Reports")
                .WithReportFileName("my_test_report")
                .WithReportFormats(ReportFormat.Html, ReportFormat.Csv, ReportFormat.Md)
                .Run();

            PrintResults(result);
        }

        private async Task<string?> GetTokenAsync()
        {
            if (string.IsNullOrWhiteSpace(_shopifyConfiguration.GrantType) ||
                string.IsNullOrWhiteSpace(_shopifyConfiguration.ClientId) ||
                string.IsNullOrWhiteSpace(_shopifyConfiguration.ClientSecret) ||
                string.IsNullOrWhiteSpace(_shopifyConfiguration.Scope))
            {
                throw new GenericNullException(Constants.ErrorMessages.ErrorWithNull);
            }

            var tokenRequest = new TokenRequestModel
            {
                GrantType = _shopifyConfiguration.GrantType,
                ClientId = _shopifyConfiguration.ClientId,
                ClientSecret = _shopifyConfiguration.ClientSecret,
                Scope = _shopifyConfiguration.Scope,
                RetryCount = _shopifyConfiguration.RetryCount,
                TokenUrl = _shopifyConfiguration.TokenUrl,
                ExistingToken = string.Empty
            };

            var token = await _genericTokenClient.GetTokenAsync(tokenRequest);

            if (string.IsNullOrEmpty(token))
                throw new GenericException(Constants.ErrorMessages.InvalidOrNullToken);

            return token;
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

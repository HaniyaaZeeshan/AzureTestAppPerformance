using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureTestAppPerformance.Services;

namespace AzureTestAppPerformance.Function
{
    public class GetSaleOrdersFunction
    {
        private readonly ISalesOrderService _salesOrderService;
        public GetSaleOrdersFunction(ISalesOrderService salesOrderService)
        {
            _salesOrderService = salesOrderService;
        }

        [FunctionName("GetSaleOrders")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Fetching sales orders...");

            var orders = await _salesOrderService.GetAllOrdersAsync();
            return new OkObjectResult(orders);
        }
    }
}

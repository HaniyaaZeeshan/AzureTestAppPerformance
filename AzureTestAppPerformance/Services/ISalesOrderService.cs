using AzureTestAppPerformance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTestAppPerformance.Services
{
    public interface ISalesOrderService
    {
        Task<SalesOrderRequest> GetSalesOrderRequestAsync();
    }
}

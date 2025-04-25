using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTestAppPerformance.Model
{
    public class SalesOrder
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
    }
}

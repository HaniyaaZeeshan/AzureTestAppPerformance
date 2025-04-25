using AzureTestAppPerformance.Model;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTestAppPerformance.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        public async Task<IEnumerable<SalesOrder>> GetAllOrdersAsync()
        {
            var faker = new Faker<SalesOrder>()
              .RuleFor(o => o.Id, f => f.IndexFaker + 1)  
              .RuleFor(o => o.Customer, f => f.Person.FullName)  
              .RuleFor(o => o.Amount, f => f.Random.Decimal(50m, 500m))  // Generates a random amount between 50 and 500
              .RuleFor(o => o.Status, f => f.PickRandom(new[] { "Completed", "Pending", "Cancelled", "Shipped" }));  

           
            var salesOrders = faker.Generate(50); //how many data

            return await Task.FromResult(salesOrders);

            // FOR HARDCODED DATA
            //return await Task.FromResult(new List<SalesOrder>
            //{
            //    new SalesOrder { Id = 1, Customer = "John Doe", Amount = 100.50, Status = "Completed" },
            //    new SalesOrder { Id = 2, Customer = "Jane Smith", Amount = 200.75, Status = "Pending" }
            //});
        }
    }
}

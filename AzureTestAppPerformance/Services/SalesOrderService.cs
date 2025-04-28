using Bogus;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureTestAppPerformance.Model;

namespace AzureTestAppPerformance.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        public async Task<SalesOrderRequest> GetSalesOrderRequestAsync()
        {
            var faker = new Faker<SalesOrderRequest>()
                .RuleFor(r => r.CurrentCompany, f => f.Company.CompanyName())  // Random company name
                .RuleFor(r => r.SalesHeadersList, f => GenerateSalesHeaders(f));

            var salesOrderRequest = faker.Generate(); // Generate a single SalesOrderRequest with random data

            return await Task.FromResult(salesOrderRequest);
        }

        private List<SalesHeader> GenerateSalesHeaders(Faker f)
        {
            return f.Make(3, () => new SalesHeader()  // Generate 3 SalesHeaders for this example
            {
                ExternalOrderId = f.Commerce.Ean13(),
                SalesOrderPoolId = f.Random.AlphaNumeric(8),
                CurrencyCode = f.Finance.Currency().Code,
                OrderClassificationId = f.Random.AlphaNumeric(5),
                InvoiceCustomerAccountNumber = f.Random.Int(1000, 9999).ToString(),
                CustomersOrderReference = f.Lorem.Word(),
                DeliveryAddressCity = f.Address.City(),
                DeliveryAddressCountryRegionId = f.Address.CountryCode(),
                DeliveryAddressDescription = f.Lorem.Sentence(),
                DeliveryAddressName = f.Name.FullName(),
                DeliveryAddressStateId = f.Address.State(),
                DeliveryAddressStreet = f.Address.StreetAddress(),
                DeliveryAddressZipCode = f.Address.ZipCode(),
                Email = f.Internet.Email(),
                OrderDate = f.Date.Past(1),
                OrderingCustomerAccountNumber = f.Random.Int(1000, 9999).ToString(),
                Phone = f.Phone.PhoneNumber(),
                RequestedShippingDate = f.Date.Future(1),
                SalesOrderOriginCode = f.Random.AlphaNumeric(5),
                ModeOfDelivery = f.Random.ArrayElement(new[] { "Air", "Sea", "Land" }),
                OrderResponsiblePersonnelNumber = f.Random.Int(1000, 9999).ToString(),
                OrderTakePersonnelNumber = f.Random.Int(1000, 9999).ToString(),
                SalesType = f.PickRandom(new[] { "Retail", "Wholesale", "Direct" }),
                MarkupTransList = GenerateMarkupTrans(f),
                PaymentList = GeneratePayments(f),
                SalesLinesList = GenerateSalesLines(f)
            }).ToList();
        }

        private List<MarkupTrans> GenerateMarkupTrans(Faker f)
        {
            return f.Make(2, () => new MarkupTrans()
            {
                MarkupCode = f.Random.AlphaNumeric(6),
                MarkupValue = f.Finance.Amount(1, 20),
                TransTxt = f.Lorem.Sentence()
            }).ToList();
        }

        private List<Payment> GeneratePayments(Faker f)
        {
            return f.Make(2, () => new Payment()
            {
                PaymentAmount = f.Finance.Amount(50, 500),
                PaymentType = f.PickRandom(new[] { "Credit Card", "Wire Transfer", "PayPal" }),
                PaymentReference = f.Random.Guid().ToString(),
                PaymentKind = f.PickRandom(new[] { "Prepaid", "Postpaid" })
            }).ToList();
        }

        private List<SalesLine> GenerateSalesLines(Faker f)
        {
            return f.Make(3, () => new SalesLine()
            {
                ExternalLineId = f.Commerce.Ean13(),
                ItemNumber = f.Commerce.ProductName(),
                ItemBarCode = f.Commerce.Ean13(),
                LineDescription = f.Lorem.Sentence(),
                OrderedSalesQuantity = f.Random.Decimal(1, 10),
                SalesPrice = f.Finance.Amount(50, 500),
                SalesUnitSymbol = f.Random.ArrayElement(new[] { "KG", "L", "Piece" }),
                DeliveryModeCode = f.Random.AlphaNumeric(5),
                IsPayloadPrice = f.Random.Bool(),
                SalesTaxItemGroupCode = f.Random.AlphaNumeric(4),
                LineDiscountAmount = f.Finance.Amount(0, 50),
                TaxAmount = f.Finance.Amount(5, 50),
                TaxCode = f.Random.AlphaNumeric(3)
            }).ToList();
        }
    }
}

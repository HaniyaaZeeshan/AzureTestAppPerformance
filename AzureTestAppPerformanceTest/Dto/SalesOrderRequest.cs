

namespace AzureTestAppPerformanceTest.Dto
{
 public class SalesOrderRequest
{
    public string CurrentCompany { get; set; }
    public List<SalesHeader> SalesHeadersList { get; set; }
}

public class SalesHeader
{
    public string ExternalOrderId { get; set; }
    public string SalesOrderPoolId { get; set; }
    public string CurrencyCode { get; set; }
    public string OrderClassificationId { get; set; }
    public string InvoiceCustomerAccountNumber { get; set; }
    public string CustomersOrderReference { get; set; }
    public string DeliveryAddressCity { get; set; }
    public string DeliveryAddressCountryRegionId { get; set; }
    public string DeliveryAddressDescription { get; set; }
    public string DeliveryAddressName { get; set; }
    public string DeliveryAddressStateId { get; set; }
    public string DeliveryAddressStreet { get; set; }
    public string DeliveryAddressZipCode { get; set; }
    public string Email { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderingCustomerAccountNumber { get; set; }
    public string Phone { get; set; }
    public DateTime RequestedShippingDate { get; set; }
    public string SalesOrderOriginCode { get; set; }
    public string ModeOfDelivery { get; set; }
    public string OrderResponsiblePersonnelNumber { get; set; }
    public string OrderTakePersonnelNumber { get; set; }
    public string SalesType { get; set; }
    public List<MarkupTrans> MarkupTransList { get; set; }
    public List<Payment> PaymentList { get; set; }
    public List<SalesLine> SalesLinesList { get; set; }
}

public class MarkupTrans
{
    public string MarkupCode { get; set; }
    public decimal MarkupValue { get; set; }
    public string TransTxt { get; set; }
}

public class Payment
{
    public decimal PaymentAmount { get; set; }
    public string PaymentType { get; set; }
    public string PaymentReference { get; set; }
    public string PaymentKind { get; set; }
}

public class SalesLine
{
    public string ExternalLineId { get; set; }
    public string ItemNumber { get; set; }
    public string ItemBarCode { get; set; }
    public string LineDescription { get; set; }
    public decimal OrderedSalesQuantity { get; set; }
    public decimal SalesPrice { get; set; }
    public string SalesUnitSymbol { get; set; }
    public string DeliveryModeCode { get; set; }
    public bool IsPayloadPrice { get; set; }
    public string SalesTaxItemGroupCode { get; set; }
    public decimal LineDiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public string TaxCode { get; set; }
}
}
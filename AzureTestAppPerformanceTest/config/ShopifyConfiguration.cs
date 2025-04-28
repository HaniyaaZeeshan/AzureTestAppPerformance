namespace AzureTestAppPerformanceTest.config
{
    public class ShopifyConfiguration : IShopifyConfiguration
    {
        public string GrantType { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public string SubscriptionId { get; set; } = string.Empty;
        public int RetryCount { get; set; }
        public string TokenUrl { get; set; } = string.Empty;
    }
}

namespace AzureTestAppPerformanceTest.config
{
    public interface IShopifyConfiguration
    {
        string GrantType { get; }
        string ClientId { get; }
        string ClientSecret { get; }
        string SubscriptionId { get; }
        string Scope { get; }
        int RetryCount { get; }
        string TokenUrl { get; }
    }
}

using AzureTestAppPerformanceTest.config;

namespace NBomberPOC;

public class App
{
    private readonly IShopifyConfiguration _shopifyConfig;

    public App(IShopifyConfiguration shopifyConfig)
    {
        _shopifyConfig = shopifyConfig;
    }

    public void Run()
    {
    }
}

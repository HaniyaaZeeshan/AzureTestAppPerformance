using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureTestAppPerformanceTest.Services
{
    public interface IHttpService
    {
        Task AddAuthorizationHeader(string token);
        Task AddSubscriptionKeyHeader(string subscriptionKey);
        Task<T?> GetAsync<T>(string endpoint);
        Task<List<T>?> GetListAsync<T>(string endpoint);  
        Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, string token, string subscriptionKey);
        Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data);
        Task<TResponse?> PatchAsync<TRequest, TResponse>(string endpoint, TRequest data);
    }
}

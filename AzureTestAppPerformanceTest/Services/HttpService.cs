using NATS.Client;
using Renci.SshNet.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzureTestAppPerformanceTest.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task AddAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Method to add Ocp-Apim-Subscription-Key header
        public async Task AddSubscriptionKeyHeader(string subscriptionKey)
        {
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
        }
        public async Task<T?> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            return await HandleResponse<T>(response);
        }

        public async Task<List<T>?> GetListAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            return await HandleResponse<List<T>>(response);  // Handle the response and return the list
        }
        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var response = await _httpClient.PostAsync(endpoint, SerializeContent(data));
            return await HandleResponse<TResponse>(response);
        }

        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var response = await _httpClient.PutAsync(endpoint, SerializeContent(data));
            return await HandleResponse<TResponse>(response);
        }

        public async Task<TResponse?> PatchAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint)
            {
                Content = SerializeContent(data)
            };

            var response = await _httpClient.SendAsync(request);
            return await HandleResponse<TResponse>(response);
        }

        private static StringContent SerializeContent<T>(T data)
        {
            var json = JsonSerializer.Serialize(data);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static async Task<T?> HandleResponse<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode) return default;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}

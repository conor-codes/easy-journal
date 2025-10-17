using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace easy_journal.Services.Http
{

    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public HttpService() 
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
        }

        public async Task<T> GetAsync<T>(string url, Dictionary<string, string> headers = null)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                AddHeaders(request, headers);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content, _jsonOptions);
            }
            catch (HttpRequestException ex)
            {
                // Log error
                throw new Exception($"HTTP request failed: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                // Log error
                throw new Exception($"JSON deserialization failed: {ex.Message}", ex);
            }
        }

        private void AddHeaders(HttpRequestMessage request, Dictionary<string, string> headers)
        {
            if (headers == null) return;

            foreach (var header in headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        public Task<string> GetStringAsync(string url, Dictionary<string, string> headers = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> PostAsync<T>(string url, object body, Dictionary<string, string> headers = null)
        {
            throw new NotImplementedException();
        }
    }
}

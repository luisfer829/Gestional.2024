
using System.Text;
using System.Text.Json;

namespace Gestional.Frontend.Repositories
{
    public class Repository : IRepository
    {
        private readonly HttpClient _httpClient;
        private JsonSerializerOptions _jsonDefaultOptions => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        public Repository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<httResponseWrapper<T>> GetAsync<T>(string url)
        {
            var responseHttp = await _httpClient.GetAsync(url);
            if(responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswer<T>(responseHttp); 
                return new httResponseWrapper<T>(response, false, responseHttp);
            }
            return new httResponseWrapper<T>(default, true, responseHttp);
        }

        public async Task<httResponseWrapper<object>> PostAsync<T>(string url, T model)
        {
            var messajeJson = JsonSerializer.Serialize(model);
            var messajeContent = new StringContent(messajeJson, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, messajeContent);
            return new httResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<httResponseWrapper<TActionResponse>> PostAsync<T, TActionResponse>(string url, T model)
        {
            var messajeJson = JsonSerializer.Serialize(model); 
            var messajeContent = new StringContent(messajeJson, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, messajeContent);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswer<TActionResponse>(responseHttp);
                return new httResponseWrapper<TActionResponse>(response, false, responseHttp);
            }
            return new httResponseWrapper<TActionResponse>(default, true, responseHttp);
        }
        private async Task<T> UnserializeAnswer<T>(HttpResponseMessage responseHttp)
        {
            var response = await responseHttp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(response, _jsonDefaultOptions)!;
        }

    }
}

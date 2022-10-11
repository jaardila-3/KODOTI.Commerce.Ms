using Api.Gateway.Models;
using Api.Gateway.Models.MsCatalog.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateway.Proxies.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Api.Gateway.Proxies
{
    /// <summary>
    /// clase que implementa las llamadas a los endpoints del Ms Catalog
    /// </summary>
    public class CatalogProxy : ICatalogProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructor que recibe por IoC las variables necesarias para utilizar el proxy
        /// </summary>
        /// <param name="httpClient"></param> cliente http
        /// <param name="apiUrls"></param> IOptions lee los valores del appsettings que llena esta variable con las url de los ms
        /// <param name="httpContextAccessor"></param> intercepta las peticiones http
        public CatalogProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<DataCollection<ProductDto>> GetAllAsync(int page, int take, IEnumerable<int> clients = null)
        {
            var ids = string.Join(',', clients ?? new List<int>());

            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogUrl}v1/products?page={page}&take={take}&ids={ids}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<DataCollection<ProductDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<ProductDto> GetAsync(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CatalogUrl}v1/products/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<ProductDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }

}

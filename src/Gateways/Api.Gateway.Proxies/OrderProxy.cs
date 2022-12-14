using Api.Gateway.Models;
using Api.Gateway.Models.MsOrder.Commands;
using Api.Gateway.Models.MsOrder.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateway.Proxies.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Api.Gateway.Proxies
{
    /// <summary>
    /// clase que implementa las llamadas a los endpoints del Ms Order
    /// </summary>
    public class OrderProxy : IOrderProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructor que recibe por IoC las variables necesarias para utilizar el proxy
        /// </summary>
        /// <param name="httpClient"></param> cliente http
        /// <param name="apiUrls"></param> IOptions lee los valores del appsettings que llena esta variable con las url de los ms
        /// <param name="httpContextAccessor"></param> intercepta las peticiones http
        public OrderProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        public async Task<DataCollection<OrderDto>> GetAllAsync(int page, int take)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.OrderUrl}v1/orders?page={page}&take={take}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<DataCollection<OrderDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<OrderDto> GetAsync(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.OrderUrl}v1/orders/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<OrderDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task CreateAsync(OrderCreateCommand command)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(command),
                Encoding.UTF8,
                "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiUrls.OrderUrl}v1/orders", content);
            request.EnsureSuccessStatusCode();
        }
    }

}

using Api.Gateway.Models;
using Api.Gateway.Models.MsOrder.Commands;
using Api.Gateway.Models.MsOrder.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Api.Gateway.WebClient.Proxy.Contracts;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;

namespace Api.Gateway.WebClient.Proxy
{
    /// <summary>
    /// Clase que implementa la conexión o proxy con el 
    /// Api.Gateway.WebClient y posterior con el Micro servicio de Order
    /// </summary>
    public class OrderProxy : IOrderProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public OrderProxy(HttpClient httpClient, ApiGatewayUrl apiGatewayUrl, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        /// <summary>
        /// Obtiene una lista de ordenes
        /// </summary>
        /// <param name="page"></param> número de página
        /// <param name="take"></param> Cantidad de registros por página
        /// <returns></returns>
        public async Task<DataCollection<OrderDto>> GetAllAsync(int page, int take)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}orders?page={page}&take={take}");
            request.EnsureSuccessStatusCode();

            var contenido = JsonSerializer.Deserialize<DataCollection<OrderDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return contenido;
        }

        /// <summary>
        /// obtiene un registro de una orden
        /// </summary>
        /// <param name="id"></param> id de la orden
        /// <returns></returns>
        public async Task<OrderDto> GetAsync(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}orders/{id}");
            request.EnsureSuccessStatusCode();

            var contenido = JsonSerializer.Deserialize<OrderDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return contenido;
        }

        /// <summary>
        /// Crea una orden
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task CreateAsync(OrderCreateCommand command)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(command),
                Encoding.UTF8,
                "application/json"
            );

            var request = await _httpClient.PostAsync($"{_apiGatewayUrl}orders", content);
            request.EnsureSuccessStatusCode();
        }
    }

}

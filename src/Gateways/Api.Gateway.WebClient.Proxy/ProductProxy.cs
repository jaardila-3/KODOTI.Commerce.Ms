using Api.Gateway.Models;
using Api.Gateway.Models.MsCatalog.DTOs;
using Api.Gateway.WebClient.Proxy.Config;
using Api.Gateway.WebClient.Proxy.Contracts;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Api.Gateway.WebClient.Proxy
{
    /// <summary>
    /// Clase que implementa la conexión o proxy con el 
    /// Api.Gateway.WebClient y posterior con el Micro servicio de Catalog
    /// </summary>
    public class ProductProxy : IProductProxy
    {
        private readonly string _apiGatewayUrl;
        private readonly HttpClient _httpClient;

        public ProductProxy(
            HttpClient httpClient,
            ApiGatewayUrl apiGatewayUrl,
            IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiGatewayUrl = apiGatewayUrl.Value;
        }

        /// <summary>
        /// Obtiene una lista de los clientes
        /// </summary>
        /// <param name="page"></param> número de página
        /// <param name="take"></param> Cantidad de registros por página
        /// <returns></returns>
        public async Task<DataCollection<ProductDto>> GetAllAsync(int page, int take)
        {
            var request = await _httpClient.GetAsync($"{_apiGatewayUrl}products?page={page}&take={take}");
            request.EnsureSuccessStatusCode();

            var contenido = JsonSerializer.Deserialize<DataCollection<ProductDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return contenido;
        }
    }

}

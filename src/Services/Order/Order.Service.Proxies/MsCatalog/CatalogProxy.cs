using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Order.Service.Proxies.Contracts;
using Order.Service.Proxies.MsCatalog.Commands;
using System.Text;
using System.Text.Json;

namespace Order.Service.Proxies.MsCatalog
{
    public class CatalogProxy : ICatalogProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;
        /// <summary>
        /// Mediante IoC y IOptions pasamos los parámetros inicializados
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="apiUrls"></param>
        public CatalogProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls/*, IHttpContextAccessor httpContextAccessor*/)
        {
            //httpClient.AddBearerToken(httpContextAccessor);
            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        /// <summary>
        /// Método que va a la acción update del Ms Catalog con el modelo del command serializado en json
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task UpdateStockAsync(ProductInStockUpdateStockCommand command)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(command),
                Encoding.UTF8,
                "application/json"
            );

            //se agrega la url, el endpoint y la data
            var request = await _httpClient.PutAsync(_apiUrls.CatalogUrl + "v1/stocks", content);
            //si el estado no es satisfactorio arroja error
            request.EnsureSuccessStatusCode();
        }
    }
}

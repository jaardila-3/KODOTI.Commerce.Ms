using Api.Gateway.Models;
using Api.Gateway.Models.MsCustomer.DTOs;
using Api.Gateway.Proxies.Config;
using Api.Gateway.Proxies.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Api.Gateway.Proxies
{
    /// <summary>
    /// clase que implementa las llamadas a los endpoints del Ms Customer
    /// </summary>
    public class CustomerProxy : ICustomerProxy
    {
        private readonly ApiUrls _apiUrls;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructor que recibe por IoC las variables necesarias para utilizar el proxy
        /// </summary>
        /// <param name="httpClient"></param> cliente http
        /// <param name="apiUrls"></param> IOptions lee los valores del appsettings que llena esta variable con las url de los ms
        /// <param name="httpContextAccessor"></param> intercepta las peticiones http
        public CustomerProxy(HttpClient httpClient, IOptions<ApiUrls> apiUrls, IHttpContextAccessor httpContextAccessor)
        {
            httpClient.AddBearerToken(httpContextAccessor);

            _httpClient = httpClient;
            _apiUrls = apiUrls.Value;
        }

        /// <summary>
        /// Retrieve All data customers
        /// </summary>
        /// <param name="page"></param>
        /// <param name="take"></param>
        /// <param name="clients"></param>
        /// <returns></returns>
        public async Task<DataCollection<ClientDto>> GetAllAsync(int page, int take, IEnumerable<int> clients = null)
        {
            var ids = string.Join(',', clients ?? new List<int>());

            var request = await _httpClient.GetAsync($"{_apiUrls.CustomerUrl}v1/clients?page={page}&take={take}&ids={ids}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<DataCollection<ClientDto>>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task<ClientDto> GetAsync(int id)
        {
            var request = await _httpClient.GetAsync($"{_apiUrls.CustomerUrl}v1/clients/{id}");
            request.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<ClientDto>(
                await request.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }

}

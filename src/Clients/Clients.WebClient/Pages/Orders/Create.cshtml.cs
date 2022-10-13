using Api.Gateway.Models;
using Api.Gateway.Models.MsCatalog.DTOs;
using Api.Gateway.Models.MsCustomer.DTOs;
using Api.Gateway.Models.MsOrder.Commands;
using Api.Gateway.WebClient.Proxy.Contracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clients.WebClient.Pages.Orders
{
    /// <summary>
    /// Clase que crea una nueva orden
    /// </summary>
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class CreateModel : PageModel
    {
        #region proxies del compilado hacia la ruta del gateway
        private readonly IOrderProxy _orderProxy;
        private readonly IClientProxy _clientProxy;
        private readonly IProductProxy _productProxy;
        #endregion

        #region propiedades
        public DataCollection<ProductDto> Products { get; set; }
        public DataCollection<ClientDto> Clients { get; set; } 
        #endregion

        /// <summary>
        /// Constructor con inyección de dependencias
        /// </summary>
        /// <param name="orderProxy"></param>
        /// <param name="clientProxy"></param>
        /// <param name="productProxy"></param>
        public CreateModel(IOrderProxy orderProxy, IClientProxy clientProxy, IProductProxy productProxy)
        {
            _orderProxy = orderProxy;
            _clientProxy = clientProxy;
            _productProxy = productProxy;
        }

        /// <summary>
        /// Método que obtiene los listados completos de productos y clientes
        /// para agregarlos en el dropdownlist
        /// </summary>
        /// <returns></returns>
        public async Task OnGet()
        {
            // *** Lo ideal sería implementar un Autocomplete para buscar los productos y cliente a demanda
            Products = await _productProxy.GetAllAsync(1, 100);
            Clients = await _clientProxy.GetAllAsync(1, 100);
        }

        /// <summary>
        /// Método que crea la orden
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPost([FromBody] OrderCreateCommand command)
        {
            await _orderProxy.CreateAsync(command);
            return this.StatusCode(200);
        }
    }
}

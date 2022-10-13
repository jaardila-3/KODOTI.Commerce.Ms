using Api.Gateway.Models.MsOrder.DTOs;
using Api.Gateway.WebClient.Proxy.Contracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clients.WebClient.Pages.Orders
{
    /// <summary>
    /// Clase para mostrar los detalles de cada orden
    /// </summary>
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class DetailModel : PageModel
    {
        /// <summary>
        /// variable proxy que va para el api gateway en su apartado de order
        /// </summary>
        private readonly IOrderProxy _orderProxy;
        /// <summary>
        /// clase para ser mapeada y mostrar datos en la vista
        /// </summary>
        public OrderDto Order { get; set; }

        /// <summary>
        /// constructor que inyecta el proxy
        /// </summary>
        /// <param name="orderProxy"></param>
        public DetailModel(IOrderProxy orderProxy)
        {
            _orderProxy = orderProxy;
        }

        /// <summary>
        /// Método que consulta la orden por el id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task OnGet(int id)
        {
            Order = await _orderProxy.GetAsync(id);
        }
    }
}

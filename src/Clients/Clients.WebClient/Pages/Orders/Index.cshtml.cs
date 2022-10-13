using Api.Gateway.Models;
using Api.Gateway.Models.MsOrder.DTOs;
using Api.Gateway.WebClient.Proxy.Contracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clients.WebClient.Pages.Orders
{
    /// <summary>
    /// clase para listar las ordenes
    /// </summary>
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class IndexModel : PageModel
    {
        /// <summary>
        /// variable para el proxy
        /// </summary>
        private readonly IOrderProxy _orderProxy;
        /// <summary>
        /// propiedad modelo para ser llenado y mostrar los datos
        /// </summary>
        public DataCollection<OrderDto> Orders { get; set; }
        /// <summary>
        /// propiedad página actual
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="orderProxy"></param>
        public IndexModel(IOrderProxy orderProxy)
        {
            _orderProxy = orderProxy;
        }
        /// <summary>
        /// Método que obtiene el listado de las ordenes
        /// </summary>
        /// <returns></returns>
        public async Task OnGet()
        {
            Orders = await _orderProxy.GetAllAsync(CurrentPage, 10);
        }
    }
}

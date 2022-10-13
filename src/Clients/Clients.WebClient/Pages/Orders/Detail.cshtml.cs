using Api.Gateway.Models.MsOrder.DTOs;
using Api.Gateway.WebClient.Proxy.Contracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clients.WebClient.Pages.Orders
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class DetailModel : PageModel
    {
        private readonly IOrderProxy _orderProxy;
        public OrderDto Order { get; set; }

        public DetailModel(IOrderProxy orderProxy)
        {
            _orderProxy = orderProxy;
        }

        public async Task OnGet(int id)
        {
            Order = await _orderProxy.GetAsync(id);
        }
    }
}

using Api.Gateway.Models;
using Api.Gateway.Models.MsOrder.DTOs;
using Api.Gateway.WebClient.Proxy.Contracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clients.WebClient.Pages.Orders
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class IndexModel : PageModel
    {
        private readonly IOrderProxy _orderProxy;

        public DataCollection<OrderDto> Orders { get; set; }
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public IndexModel(IOrderProxy orderProxy)
        {
            _orderProxy = orderProxy;
        }

        public async Task OnGet()
        {
            Orders = await _orderProxy.GetAllAsync(CurrentPage, 10);
        }
    }
}

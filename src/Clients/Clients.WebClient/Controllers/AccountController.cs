using Clients.WebClient.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Clients.WebClient.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _authenticationUrl;

        /// <summary>
        /// constructor que llena por IoC la variable _authenticationUrl
        /// </summary>
        /// <param name="configuration"></param>
        public AccountController(IConfiguration configuration)
        {
            _authenticationUrl = configuration.GetValue<string>("AuthenticationUrl");
        }

        /// <summary>
        /// Este método redirige al proyecto de authentication adjuntando en el get una url de retorno
        /// el cual se especifica como la dirección base actual de este proyecto
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return Redirect(_authenticationUrl + $"?ReturnBaseUrl={this.Request.Scheme}://{this.Request.Host}/");
        }

        /// <summary>
        /// Método que recibe el token desde el proyecto de clients.authentication
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Connect(string access_token)
        {
            //separamos el token por el punto
            var token = access_token.Split('.');

            //convierto a base 64 el segundo elemento del array que es donde están los claims
            var base64Content = Convert.FromBase64String(token[1]);

            //se deserializa el contenido de base64Content
            var user = JsonSerializer.Deserialize<AccessTokenUserInformation>(base64Content);

            //generamos los claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.nameid),
                new Claim(ClaimTypes.Name, user.unique_name),
                new Claim(ClaimTypes.Email, user.email),
                new Claim("access_token", access_token) //lleno con el token completo
            };

            //nos autenticamos en el sistema con las cookies
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //vencimiento de las cookies
            var authProperties = new AuthenticationProperties { IssuedUtc = DateTime.UtcNow.AddHours(10) };

            //me autentico en el sistema
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return Redirect("~/");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/");
        }
    }

}

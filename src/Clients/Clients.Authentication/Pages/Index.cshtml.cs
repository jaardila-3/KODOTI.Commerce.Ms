using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using Clients.Authentication.Models;

namespace Clients.Authentication.Pages
{
    public class IndexModel : PageModel
    {
        /// <summary>
        /// variable que recupera la url de este proyecto de autenticación en el appsettings
        /// </summary>
        private readonly string _identityUrl;

        #region Propiedades
        /// <summary>
        /// propiedad utilizada para recuperar la url de retorno
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string ReturnBaseUrl { get; set; }
        /// <summary>
        /// Estructura que recibe los datos de la vista index donde esta el formulario de logueo
        /// </summary>
        [BindProperty]
        public LoginViewModel model { get; set; }
        /// <summary>
        /// propiedad para validar si el acceso no fue exitoso y dar alerta en la vista
        /// </summary>
        public bool HasInvalidAccess { get; set; } 
        #endregion

        public IndexModel(IConfiguration configuration)
        {
            _identityUrl = configuration.GetValue<string>("IdentityUrl");
        }

        public void OnGet()
        {

        }

        /// <summary>
        /// Método post que utiliza httpClient
        /// serializamos los parámetros de entrada : propiedad model
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPost()
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(model),
                    Encoding.UTF8,
                    "application/json"
                );

                //pasamos los datos al Ms Identity.Api el cual nos genera el token
                var request = await client.PostAsync(_identityUrl + "v1/identity/authentication", content);

                //si NO fue exitoso
                if (!request.IsSuccessStatusCode)
                {
                    //fue invalido el acceso
                    HasInvalidAccess = true;
                    return Page();
                }

                //deserializo el token
                var result = JsonSerializer.Deserialize<IdentityAccess>(await request.Content.ReadAsStringAsync(),
                                                                        new JsonSerializerOptions
                                                                        {
                                                                            //invalide el sensitive case
                                                                            PropertyNameCaseInsensitive = true
                                                                        }
                );

                //retomamos la url de retorno que se envió desde el re-direccionamiento de login el proyecto clients.webclient
                //y le concatenamos el contralor el método y el parámetro que necesita para acceder al sistema
                return Redirect(ReturnBaseUrl + $"account/connect?access_token={result.AccessToken}");
            }
        }
    }
}
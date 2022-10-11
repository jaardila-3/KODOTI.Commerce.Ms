using Microsoft.AspNetCore.Http;

namespace Order.Service.Proxies
{
    public static class HttpClientTokenExtension
    {
        /// <summary>
        /// Método de extensión que se aplicará al httpClient de CatalogProxy para llevar el token al otro Ms
        /// </summary>
        /// <param name="client"></param>
        /// <param name="context"></param>
        public static void AddBearerToken(this HttpClient client, IHttpContextAccessor context)
        {
            //si el contexto o request esta autenticado Y en sus headers tiene Authorization
            if (context.HttpContext.User.Identity.IsAuthenticated && context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                //recupera el token
                var token = context.HttpContext.Request.Headers["Authorization"].ToString();

                //si el token no es nulo o vacío lo agrega al httpClient que hará la solicitud de actualización al otro Ms
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token);
                }
            }
        }
    }
}

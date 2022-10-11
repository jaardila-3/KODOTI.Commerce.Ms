using Microsoft.AspNetCore.Http;

namespace Api.Gateway.Proxies.Config
{
    /// <summary>
    /// Clase que genera método de extension para adicionar token bearer al cliente http
    /// </summary>
    public static class HttpClientTokenExtension
    {
        /// <summary>
        /// Método de extensión que se aplicará al httpClient de CatalogProxy para llevar el token al otro Ms
        /// </summary>
        /// <param name="client"></param>
        /// <param name="context"></param>
        public static void AddBearerToken(this HttpClient client, IHttpContextAccessor context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated && context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                var token = context.HttpContext.Request.Headers["Authorization"].ToString();

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token);
                }
            }
        }
    }
}

using Microsoft.AspNetCore.Http;

namespace Api.Gateway.WebClient.Proxy.Config
{
    /// <summary>
    /// Clase que genera método de extension para adicionar token bearer al cliente http
    /// </summary>
    public static class HttpClientTokenExtension
    {
        /// <summary>
        /// Método de extensión llamado por los diferentes proxy de este proyecto
        /// y que se aplicará al httpClient para llevar el access_token generado en el cliente
        /// hacia el Api.Gateway.WebClient
        /// </summary>
        /// <param name="client"></param>
        /// <param name="context"></param>
        public static void AddBearerToken(this HttpClient client, IHttpContextAccessor context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var token = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals("access_token"))?.Value;

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);
                }
            }
        }
    }
}

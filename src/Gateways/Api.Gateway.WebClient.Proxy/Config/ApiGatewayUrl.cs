namespace Api.Gateway.WebClient.Proxy.Config
{
    /// <summary>
    /// clase para poblar con la url del Api.Gateway.WebClient
    /// </summary>
    public class ApiGatewayUrl
    {
        public ApiGatewayUrl(string url)
        {
            Value = url;
        }

        public readonly string Value;
    }
}

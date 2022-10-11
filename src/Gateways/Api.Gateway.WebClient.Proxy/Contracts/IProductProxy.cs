using Api.Gateway.Models;
using Api.Gateway.Models.MsCatalog.DTOs;

namespace Api.Gateway.WebClient.Proxy.Contracts
{
    public interface IProductProxy
    {
        Task<DataCollection<ProductDto>> GetAllAsync(int page, int take);
    }
}

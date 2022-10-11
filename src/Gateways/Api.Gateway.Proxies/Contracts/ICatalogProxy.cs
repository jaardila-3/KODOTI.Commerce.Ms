using Api.Gateway.Models;
using Api.Gateway.Models.MsCatalog.DTOs;

namespace Api.Gateway.Proxies.Contracts
{
    public interface ICatalogProxy
    {
        Task<DataCollection<ProductDto>> GetAllAsync(int page, int take, IEnumerable<int> clients = null);
        Task<ProductDto> GetAsync(int id);
    }
}

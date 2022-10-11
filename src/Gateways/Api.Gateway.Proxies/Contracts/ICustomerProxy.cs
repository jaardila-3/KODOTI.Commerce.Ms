using Api.Gateway.Models;
using Api.Gateway.Models.MsCustomer.DTOs;

namespace Api.Gateway.Proxies.Contracts
{
    public interface ICustomerProxy
    {
        Task<DataCollection<ClientDto>> GetAllAsync(int page, int take, IEnumerable<int> clients = null);
        Task<ClientDto> GetAsync(int id);
    }
}

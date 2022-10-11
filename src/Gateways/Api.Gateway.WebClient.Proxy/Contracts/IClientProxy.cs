using Api.Gateway.Models;
using Api.Gateway.Models.MsCustomer.DTOs;

namespace Api.Gateway.WebClient.Proxy.Contracts
{
    public interface IClientProxy
    {
        Task<DataCollection<ClientDto>> GetAllAsync(int page, int take);
    }
}

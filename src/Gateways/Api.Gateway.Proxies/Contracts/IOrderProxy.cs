using Api.Gateway.Models;
using Api.Gateway.Models.MsOrder.Commands;
using Api.Gateway.Models.MsOrder.DTOs;

namespace Api.Gateway.Proxies.Contracts
{
    public interface IOrderProxy
    {
        Task<DataCollection<OrderDto>> GetAllAsync(int page, int take);
        Task<OrderDto> GetAsync(int id);
        Task CreateAsync(OrderCreateCommand command);
    }
}

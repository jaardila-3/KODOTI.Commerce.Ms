using Order.Service.Queries.DTOs;
using Service.Common.Collection;

namespace Order.Service.Queries.Contracts
{
    public interface IOrderQueryService
    {
        Task<DataCollection<OrderDto>> GetAllAsync(int page, int take);
        Task<OrderDto> GetAsync(int id);
    }
}

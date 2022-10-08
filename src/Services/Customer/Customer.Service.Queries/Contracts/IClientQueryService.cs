using Customer.Service.Queries.DTOs;
using Service.Common.Collection;

namespace Customer.Service.Queries.Contracts
{
    public interface IClientQueryService
    {
        Task<DataCollection<ClientDto>> GetAllAsync(int page, int take, IEnumerable<int> clients = null);
        Task<ClientDto> GetAsync(int id);
    }
}

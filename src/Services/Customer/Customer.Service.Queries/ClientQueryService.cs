using Customer.Persistence.Database;
using Customer.Service.Queries.Contracts;
using Customer.Service.Queries.DTOs;
using Microsoft.EntityFrameworkCore;
using Service.Common.Collection;
using Service.Common.Mapping;
using Service.Common.Paging;

namespace Customer.Service.Queries
{
    public class ClientQueryService : IClientQueryService
    {
        private readonly ApplicationDbContext _context;

        public ClientQueryService(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataCollection<ClientDto>> GetAllAsync(int page, int take, IEnumerable<int> clients = null)
        {
            var collection = await _context.Clients
                .Where(x => clients == null || clients.Contains(x.ClientId))
                .OrderBy(x => x.Name)
                .GetPagedAsync(page, take);

            return collection.MapTo<DataCollection<ClientDto>>();
        }

        public async Task<ClientDto> GetAsync(int id)
        {
            return (await _context.Clients.SingleAsync(x => x.ClientId == id)).MapTo<ClientDto>();
        }
    }
}

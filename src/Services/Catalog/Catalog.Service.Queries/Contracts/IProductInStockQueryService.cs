using Catalog.Service.Queries.DTOs;
using Service.Common.Collection;

namespace Catalog.Service.Queries.Contracts
{
    public interface IProductInStockQueryService
    {
        Task<DataCollection<ProductInStockDto>> GetAllAsync(int page, int take, IEnumerable<int> products = null);
    }
}

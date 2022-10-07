using Catalog.Persistence.Database;
using Catalog.Service.Queries.Contracts;
using Catalog.Service.Queries.DTOs;
using Microsoft.EntityFrameworkCore;
using Service.Common.Collection;
using Service.Common.Mapping;
using Service.Common.Paging;

namespace Catalog.Service.Queries
{
    public class ProductQueryService : IProductQueryService
    {
        private readonly ApplicationDbContext _context;

        public ProductQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataCollection<ProductDto>> GetAllAsync(
            int page, int take, IEnumerable<int>? products = null)
        {
            var collection = await _context.Products
                .Where(x => products == null || products.Contains(x.ProductId))
                .Include(x => x.Stock)
                .OrderByDescending(x => x.ProductId)
                .GetPagedAsync(page, take);

            return collection.MapTo<DataCollection<ProductDto>>();
        }

        public async Task<ProductDto> GetAsync(int id)
        {
            return (await _context.Products.Include(x => x.Stock).SingleAsync(x => x.ProductId == id)).MapTo<ProductDto>();
        }
    }
}

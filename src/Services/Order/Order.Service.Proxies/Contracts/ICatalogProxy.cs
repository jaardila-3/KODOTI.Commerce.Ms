using Order.Service.Proxies.MsCatalog.Commands;

namespace Order.Service.Proxies.Contracts
{
    public interface ICatalogProxy
    {
        Task UpdateStockAsync(ProductInStockUpdateStockCommand command);
    }
}

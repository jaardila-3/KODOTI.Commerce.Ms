using Catalog.Common.Enumeraciones;
using MediatR;

namespace Catalog.Service.EventHandlers.Commands
{
    //clase command: no podemos retornar directamente una lista de command por eso declaramos la propiedad items como lista
    public class ProductInStockUpdateStockCommand : INotification
    {
        public IEnumerable<ProductInStockUpdateItem> Items { get; set; } = new List<ProductInStockUpdateItem>();
    }

    //clase objeto para los items
    public class ProductInStockUpdateItem
    {
        public int ProductId { get; set; }
        public int Stock { get; set; }
        public ProductInStockAction Action { get; set; }
    }
}

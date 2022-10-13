using MediatR;
using Microsoft.Extensions.Logging;
using Order.Domain;
using Order.Persistence.Database;
using Order.Service.EventHandlers.Commands;
using Order.Service.Proxies.Contracts;
using Order.Service.Proxies.Enums;
using Order.Service.Proxies.MsCatalog.Commands;

namespace Order.Service.EventHandlers
{
    public class OrderCreateEventHandler : INotificationHandler<OrderCreateCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICatalogProxy _catalogProxy;
        private readonly ILogger<OrderCreateEventHandler> _logger;

        public OrderCreateEventHandler( ApplicationDbContext context, ICatalogProxy catalogProxy, ILogger<OrderCreateEventHandler> logger)
        {
            _context = context;
            _catalogProxy = catalogProxy;
            _logger = logger;
        }

        public async Task Handle(OrderCreateCommand notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("--- New order creation started");
            var entry = new Domain.Order();

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                // 01. Prepare detail
                _logger.LogInformation("--- Preparing detail");
                PrepareDetail(entry, notification);

                // 02. Prepare header
                _logger.LogInformation("--- Preparing header");
                PrepareHeader(entry, notification);

                // 03. Create order
                _logger.LogInformation("--- Creating order");
                await _context.AddAsync(entry);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"--- Order {entry.OrderId} was created");

                // 04. Update Stocks
                //aquí se orquesta con el micro servicio de Catalog
                _logger.LogInformation("--- Updating stock");
                try
                {
                    await _catalogProxy.UpdateStockAsync(new ProductInStockUpdateStockCommand
                    {
                        Items = notification.Items.Select(x => new ProductInStockUpdateItem
                        {
                            ProductId = x.ProductId,
                            Stock = x.Quantity,
                            Action = ProductInStockAction.Substract
                        })
                    });
                }
                catch (Exception e)
                {
                    _logger.LogError("NO SE CREO LA ORDEN DEBIDO A LA FALTA DE STOCK");
                    throw;
                }                

                await transaction.CommitAsync();
            }

            _logger.LogInformation("--- New order creation ended");
        }

        private void PrepareDetail(Domain.Order entry, OrderCreateCommand notification)
        {
            entry.Items = notification.Items.Select(x => new OrderDetail
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                UnitPrice = x.Price,
                Total = x.Price * x.Quantity
            }).ToList();
        }

        private void PrepareHeader(Domain.Order entry, OrderCreateCommand notification)
        {
            // Header information
            entry.Status = Common.Enums.OrderStatus.Pending;
            entry.PaymentType = notification.PaymentType;
            entry.ClientId = notification.ClientId;
            entry.CreatedAt = DateTime.UtcNow;

            // Sum
            entry.Total = entry.Items.Sum(x => x.Total);
        }
    }

}

using Api.Gateway.Models.MsOrder.Enums;

namespace Api.Gateway.Models.MsOrder.Commands
{
    /// <summary>
    /// Comando que se le envía al Ms Order OrderController en el método Create parámetro OrderCreateCommand
    /// </summary>
    public class OrderCreateCommand
    {
        public OrderPayment PaymentType { get; set; }
        public int ClientId { get; set; }
        public IEnumerable<OrderCreateDetail> Items { get; set; } = new List<OrderCreateDetail>();
    }

    public class OrderCreateDetail
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}

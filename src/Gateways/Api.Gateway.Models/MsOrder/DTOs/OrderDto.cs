using Api.Gateway.Models.MsCustomer.DTOs;
using Api.Gateway.Models.MsOrder.Enums;

namespace Api.Gateway.Models.MsOrder.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        /// <summary>
        /// Esta propiedad es adicional para obtener los datos del cliente
        /// </summary>
        public ClientDto Client { get; set; }
        public string OrderNumber { get; set; }
        public OrderStatus Status { get; set; }
        public OrderPayment PaymentType { get; set; }
        public int ClientId { get; set; }
        public IEnumerable<OrderDetailDto> Items { get; set; } = new List<OrderDetailDto>();
        public DateTime CreatedAt { get; set; }
        public decimal Total { get; set; }
    }
}

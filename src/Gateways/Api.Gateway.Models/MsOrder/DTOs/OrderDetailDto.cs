using Api.Gateway.Models.MsCatalog.DTOs;

namespace Api.Gateway.Models.MsOrder.DTOs
{
    public class OrderDetailDto
    {
        /// <summary>
        /// Esta propiedad es adicional para obtener los datos del producto
        /// </summary>
        public ProductDto Product { get; set; }
        public int OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
    }
}

namespace Api.Gateway.Models.MsCatalog.DTOs
{
    /// <summary>
    /// Dto utilizado para el Micro servicio de Catalogo
    /// </summary>
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}

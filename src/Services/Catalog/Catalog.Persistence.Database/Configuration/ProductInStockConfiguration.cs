using Catalog.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Persistence.Database.Configuration
{
    public class ProductInStockConfiguration
    {
        public ProductInStockConfiguration(EntityTypeBuilder<ProductInStock> entityBuilder)
        {
            entityBuilder.HasIndex(x => x.ProductInStockId);//PK

            //Product by default
            var productsInStock = new List<ProductInStock>();
            var random = new Random();
            int quantityOfProducts = 100;

            for (int i = 1; i <= quantityOfProducts; i++)
            {
                productsInStock.Add(
                        //stock
                        new ProductInStock
                        {
                            ProductInStockId = i,
                            ProductId = i,
                            Stock = random.Next(0, 20)
                        }
                    );
            }
            //add initial data
            entityBuilder.HasData(productsInStock);
        }
    }
}

using Catalog.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Persistence.Database.Configuration
{
    public class ProductConfiguration
    {
        public ProductConfiguration(EntityTypeBuilder<Product> entityBuilder)
        {
            entityBuilder.HasIndex(x => x.ProductId);//PK
            entityBuilder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            entityBuilder.Property(x => x.Description).IsRequired().HasMaxLength(500);

            //seed; sembramos datos iniciales en bd
            //Product by default
            var products = new List<Product>();
            var random = new Random();
            int quantityOfProducts = 100;

            for (int i = 1; i <= quantityOfProducts; i++)
            {
                products.Add(
                    //product
                    new Product
                    {
                        ProductId = i,
                        Name = $"Product {i}",
                        Description = $"Description for product {i}",
                        Price = random.Next(100, 1000)
                    });
            }
            //add initial data
            entityBuilder.HasData(products);
        }
    }
}

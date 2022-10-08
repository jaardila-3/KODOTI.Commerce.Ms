using Catalog.Domain;
using Catalog.Persistence.Database;
using Catalog.Service.EventHandlers;
using Catalog.Service.EventHandlers.Commands;
using Catalog.Service.EventHandlers.Exceptions;
using Catalog.Tests.Config;
using Microsoft.Extensions.Logging;
using Moq;

namespace Catalog.Tests
{
    [TestClass]
    public class ProductInStockUpdateStockEventHandlerTest
    {
        //instanciamos las variables que utiliza ProductInStockUpdateStockEventHandler en su constructor como IoC
        //_logger: objeto falso creado con moq
        private ILogger<ProductInStockUpdateStockEventHandler> GetIlogger
        {
            get
            {
                return new Mock<ILogger<ProductInStockUpdateStockEventHandler>>().Object;
            }
        }
        //_context: va a base de datos en memoria
        private ApplicationDbContext context = ApplicationDbContextInMemory.Get();

        /// <summary>
        /// Método que prueba el intentar sustraer stock cuando hay existencia
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TryToSubstractStockWhenProductHasStock()
        {
            var productInStockId = 1;
            var productId = 1;
            SaveProductInStock(productInStockId, productId);
            //llamamos la clase original que se va a probar
            var command = new ProductInStockUpdateStockEventHandler(context, GetIlogger);
            //ejecutamos el método a probar y le enviamos los dos parámetros
            await command.Handle(
                new ProductInStockUpdateStockCommand
                    {
                        Items = new List<ProductInStockUpdateItem> {
                            new ProductInStockUpdateItem {
                                ProductId = productId,
                                Stock = 1,
                                Action = Common.Enumeraciones.ProductInStockAction.Substract
                            }
                        }
                    },
                new CancellationToken());
        }

        /// <summary>
        /// Método que prueba el intentar sustraer stock cuando NO hay existencia
        /// El decorador ExpectedException espera que se produzca una excepción del tipo ProductInStockUpdateStockCommandException
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(ProductInStockUpdateStockCommandException))]
        public void TryToSubstractStockWhenProductHasntStock()
        {
            var productInStockId = 2;
            var productId = 2;
            SaveProductInStock(productInStockId, productId);

            var command = new ProductInStockUpdateStockEventHandler(context, GetIlogger);

            try
            {
                command.Handle(new ProductInStockUpdateStockCommand
                {
                    Items = new List<ProductInStockUpdateItem> {
                    new ProductInStockUpdateItem {
                        ProductId = productId,
                        Stock = 2,
                        Action = Common.Enumeraciones.ProductInStockAction.Substract
                    }
                }
                }, new CancellationToken()).Wait();//espera que se ejecute porque es asincrónico
            }
            catch (AggregateException ae)
            {
                //los métodos asincrónicos devuelven una excepción del tipo AggregateException
                if (ae.GetBaseException() is ProductInStockUpdateStockCommandException)
                {
                    throw new ProductInStockUpdateStockCommandException(ae.InnerException?.Message);
                }
            }
        }

        /// <summary>
        /// Método que prueba el intentar adicionar stock cuando ya tiene existencias
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void TryToAddStockWhenProductExists()
        {
            var productInStockId = 3;
            var productId = 3;
            SaveProductInStock(productInStockId, productId);

            var command = new ProductInStockUpdateStockEventHandler(context, GetIlogger);
            command.Handle(new ProductInStockUpdateStockCommand
            {
                Items = new List<ProductInStockUpdateItem> {
                    new ProductInStockUpdateItem {
                        ProductId = productId,
                        Stock = 2,
                        Action = Common.Enumeraciones.ProductInStockAction.Add
                    }
                }
            }, new CancellationToken()).Wait();

            //validamos si el stock en bd es el esperado
            var stockBD = context.ProductsInStock.First(x => x.ProductInStockId == productInStockId).Stock;
            Assert.AreEqual(stockBD, 3);
        }

        /// <summary>
        /// Método que prueba el intentar adicionar stock cuando NO tiene existencias
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void TryToAddStockWhenProductNotExists()
        {
            var productId = 4;
            //no agregamos stock en BD
            var command = new ProductInStockUpdateStockEventHandler(context, GetIlogger);
            command.Handle(new ProductInStockUpdateStockCommand
            {
                Items = new List<ProductInStockUpdateItem> {
                    new ProductInStockUpdateItem {
                        ProductId = productId,
                        Stock = 2,
                        Action = Common.Enumeraciones.ProductInStockAction.Add
                    }
                }
            }, new CancellationToken()).Wait();

            //validamos si el stock en bd es el esperado
            var stockBD = context.ProductsInStock.First(x => x.ProductId == productId).Stock;
            Assert.AreEqual(stockBD, 2);
        }

        /// <summary>
        /// metodo que guarda existencia de 1 stock en la bd en memoria
        /// </summary>
        private void SaveProductInStock(int _productInStockId, int _productId)
        {
            //declaramos los id;
            var productInStockId = _productInStockId;
            var productId = _productId;

            // Add product
            context.ProductsInStock.Add(new ProductInStock
            {
                ProductInStockId = productInStockId,
                ProductId = productId,
                Stock = 1
            });
            //Guardamos cambios
            context.SaveChanges();
        }
    }
}
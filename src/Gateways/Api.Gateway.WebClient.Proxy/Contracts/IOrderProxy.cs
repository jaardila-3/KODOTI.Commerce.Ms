using Api.Gateway.Models;
using Api.Gateway.Models.MsOrder.Commands;
using Api.Gateway.Models.MsOrder.DTOs;

namespace Api.Gateway.WebClient.Proxy.Contracts
{
    public interface IOrderProxy
    {
        /// <summary>
        /// Este método no trae la información de los productos.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        Task<DataCollection<OrderDto>> GetAllAsync(int page, int take);

        /// <summary>
        /// Trae la información completa de la orden haciendo cruce con los diferentes microservicios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OrderDto> GetAsync(int id);

        /// <summary>
        /// Creación de órdenes de compra
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task CreateAsync(OrderCreateCommand command);
    }
}

using Api.Gateway.Models;
using Api.Gateway.Models.MsCatalog.DTOs;
using Api.Gateway.Proxies.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Gateway.WebClient.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly ICatalogProxy _catalogProxy;

        public ProductController(ICatalogProxy catalogProxy)
        {
            _catalogProxy = catalogProxy;
        }

        [HttpGet]
        public async Task<DataCollection<ProductDto>> GetAll(int page = 1, int take = 10)
        {
            return await _catalogProxy.GetAllAsync(page, take);
        }

        [HttpGet("{id}")]
        public async Task<ProductDto> Get(int id)
        {
            return await _catalogProxy.GetAsync(id);
        }
    }
}

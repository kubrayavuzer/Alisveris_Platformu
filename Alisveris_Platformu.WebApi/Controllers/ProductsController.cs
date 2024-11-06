using Alisveris_Platformu.Business.Operations.Product;
using Alisveris_Platformu.Business.Operations.Product.Dtos;
using Alisveris_Platformu.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineAlışverişPlatformu.WebApi.Jwt;

namespace Alisveris_Platformu.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("Add")]
        [Authorize]


        public async Task<IActionResult> AddProduct(AddProductRequest request)
        {

            var addProductDto = new AddProductDto
            {
                ProductName = request.ProductName,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                
            };

            var result = await _productService.AddProduct(addProductDto);

            if (result.IsSucceed)
                return Ok();
            else
                return BadRequest(result.Message);
        }
    }
}

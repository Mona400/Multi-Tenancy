using Microsoft.AspNetCore.Mvc;
using MultiTenency.Dtos;
using MultiTenency.Models;
using MultiTenency.Services;

namespace MultiTenency.Controllers
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

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("id:int")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var products = await _productService.GetByIdAsync(id);
            if (products is null)
            {
                return NotFound();
            }
            return Ok(products);
        }
        [HttpPost]
        public async Task<IActionResult> CreatedAsync(CreateProductDto createProductDto)
        {
            Product product = new()
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Rate = createProductDto.Rate,
            };
            var createdProduct = await _productService.CreateAsync(product);
            return Ok(createdProduct);
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsInventoryApi.Data;
using ProductsInventoryApi.DTOs;
using ProductsInventoryApi.Models;

namespace ProductsInventoryApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly ProductDbContext _context;
        private readonly IMapper _mapper;

        public ProductController(ProductDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetProducts()
        {
            var products = await _context.Products.OrderBy(p => p.Name).ToListAsync();
            foreach(var prod in products)
            {
                Console.WriteLine("Product Id: " + prod.Id);
            }
            return _mapper.Map<List<ProductDto>>(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if(product == null) return NotFound();

            return _mapper.Map<ProductDto>(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            _context.Products.Add(product);

            var result = await _context.SaveChangesAsync() > 0;

            if(!result) return BadRequest("Could not save changed to the database");

            return CreatedAtAction(nameof(GetProductById), new{product.Id}, _mapper.Map<ProductDto>(product));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto updateProductDto)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if(product == null) return NotFound();

            bool hasChanges = updateProductDto.Name != product.Name ||
                              updateProductDto.Brand != product.Brand ||
                              updateProductDto.Price != product.Price;

            if (!hasChanges) return Ok();

            product.Name = updateProductDto.Name ?? product.Name;
            product.Brand = updateProductDto.Brand ?? product.Brand;
            product.Price = updateProductDto.Price ?? product.Price;

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return Ok();

            return BadRequest("Could not update the product");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if(product == null) return NotFound();

            _context.Products.Remove(product);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return Ok();

            return BadRequest("Could not delete the product");

        }
    }
}
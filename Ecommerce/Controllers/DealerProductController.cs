using Microsoft.AspNetCore.Mvc;
using Ecommerce.Model;
using Ecommerce.Interface;


namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerProductController : ControllerBase
    {
        private readonly IDealerProductRepository _productRepository;

        public DealerProductController(IDealerProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost("add")]
        public IActionResult AddProduct([FromBody] Product product)
        {
            if (product == null || product.DealerID <= 0 || string.IsNullOrEmpty(product.ProductName) || product.Price <= 0 || product.Stock < 0)
                return BadRequest(new { error = "Invalid product data." });

            try
            {
                _productRepository.AddProduct(product);
                return Ok(new { message = "Product added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpGet("dealer/{dealerId}")]
        public IActionResult GetDealerProducts(int dealerId)
        {
            if (dealerId <= 0)
                return BadRequest(new { error = "Invalid dealer ID." });

            try
            {
                var products = _productRepository.GetDealerProducts(dealerId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            if (id <= 0)
                return BadRequest(new { Significance = "Invalid product ID." });

            try
            {
                var product = _productRepository.GetProduct(id);
                if (product == null)
                    return NotFound(new { error = $"Product with ID {id} does not exist." });

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            if (id <= 0 || product == null || string.IsNullOrEmpty(product.ProductName) || product.Price <= 0 || product.Stock < 0)
                return BadRequest(new { error = "Invalid product data." });

            try
            {
                bool updated = _productRepository.UpdateProduct(id, product);
                if (!updated)
                    return NotFound(new { error = $"Product with ID {id} does not exist." });

                return Ok(new { message = "Product updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            if (id <= 0)
                return BadRequest(new { error = "Invalid product ID." });

            try
            {
                bool deleted = _productRepository.DeleteProduct(id);
                if (!deleted)
                    return NotFound(new { error = $"Product with ID {id} does not exist." });

                return Ok(new { message = "Product deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
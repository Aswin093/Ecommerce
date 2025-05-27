using Microsoft.AspNetCore.Mvc;
using Ecommerce.Interface;


namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        private readonly IProductImagesRepository _productImagesRepository;

        public ProductImagesController(IProductImagesRepository productImagesRepository)
        {
            _productImagesRepository = productImagesRepository;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetImagesByProductId(int productId)
        {
            try
            {
                var images = await _productImagesRepository.GetImagesByProductId(productId);
                return Ok(images);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving images", error = ex.Message });
            }
        }

        [HttpGet("Details/{productId}")]
        public async Task<IActionResult> GetProductDetails(int productId)
        {
            try
            {
                var product = await _productImagesRepository.GetProductDetails(productId);
                if (product == null)
                {
                    return NotFound(new { message = "Product not found." });
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving product", error = ex.Message });
            }
        }
        [HttpPost("{productId}/image")]
        public async Task<IActionResult> AddImagePath(int productId, [FromBody] string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return BadRequest("Image path cannot be empty.");
            }

            var result = await _productImagesRepository.AddImagePath(productId, imagePath);
            if (result)
            {
                return Ok("Image path added successfully.");
            }

            return StatusCode(500, "Failed to add image path.");
        }
    }
}
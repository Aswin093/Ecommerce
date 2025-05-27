using Microsoft.AspNetCore.Mvc;
using Ecommerce.Interface;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartProductController : ControllerBase
    {
        private readonly ICartProductRepository _cartProductRepository;

        public CartProductController(ICartProductRepository cartProductRepository)
        {
            _cartProductRepository = cartProductRepository;
        }

        [HttpGet("customer/{customerId}")]
        public IActionResult GetCartItemsByCustomer(int customerId)
        {
            try
            {
                var cartItems = _cartProductRepository.GetCartItemsByCustomer(customerId);
                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Model;
using Ecommerce.Interface;
using System.Collections.Generic;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        //insert
        [HttpPost("add")]
        public IActionResult AddToCart([FromBody] AddToCart cartItem)
        {
            if (cartItem == null || cartItem.CustomerID == 0 || cartItem.ProductID == 0)
                return BadRequest("Invalid cart data.");

            try
            {
                _cartRepository.AddToCart(cartItem);
                return Ok(new { message = "Cart updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        //select
        [HttpGet("customer/{customerID}")]
        public IActionResult GetCartByCustomer(int customerID)
        {
            if (customerID <= 0)
                return BadRequest("Invalid customer ID.");

            try
            {
                var cartItems = _cartRepository.GetCartByCustomer(customerID);
                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        //update
        [HttpPut("update")]
        public IActionResult UpdateCartQuantity([FromBody] UpdateCartRequest request)
        {
            if (request == null || request.CartID <= 0 || request.ProductID <= 0 || request.Quantity <= 0)
                return BadRequest("Invalid cart data.");

            try
            {
                bool updated = _cartRepository.UpdateCartQuantity(request);
                if (!updated)
                    return NotFound("Cart item not found or already checked out.");

                return Ok(new { message = "Cart quantity updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        //delete
        [HttpDelete("remove/{cartID}")]
        public IActionResult RemoveFromCart(int cartID)
        {
            if (cartID <= 0)
                return BadRequest("Invalid cart ID.");

            try
            {
                bool removed = _cartRepository.RemoveFromCart(cartID);
                if (!removed)
                    return NotFound("Cart item not found or already checked out.");

                return Ok(new { message = "Item removed from cart successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
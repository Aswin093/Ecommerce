using Microsoft.AspNetCore.Mvc;
using Ecommerce.Model;
using Ecommerce.Interface;
using System;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] OrderRequestDto orderRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (orderRequest.Quantity <= 0)
            {
                return BadRequest(new { Message = "Quantity must be greater than zero." });
            }
            if (orderRequest.AddressId <= 0)
            {
                return BadRequest(new { Message = "Invalid AddressId." });
            }

            try
            {
                int orderId = _orderRepository.CreateOrder(orderRequest);
                return Ok(new { Message = "Order created successfully.", OrderID = orderId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{customerId}")]
        public IActionResult GetOrdersByCustomer(int customerId)
        {
            try
            {
                var orders = _orderRepository.GetOrdersByCustomer(customerId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("{orderId}")]
        public IActionResult DeleteOrder(int orderId)
        {
            try
            {
                int deletedOrderId = _orderRepository.DeleteOrder(orderId);
                return Ok(new { Message = "Order deleted and stock restored successfully.", OrderID = deletedOrderId });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Order not found"))
                {
                    return NotFound(new { Message = "Order not found or already deleted." });
                }
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
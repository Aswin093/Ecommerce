using Microsoft.AspNetCore.Mvc;
using Ecommerce.Model;
using Ecommerce.Interface;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAddressController : ControllerBase
    {
        private readonly ICustomerAddressRepository _customerAddressRepository;

        public CustomerAddressController(ICustomerAddressRepository customerAddressRepository)
        {
            _customerAddressRepository = customerAddressRepository;
        }

        [HttpGet("customer/{customerId}")]
        public IActionResult GetCustomerAddresses(int customerId)
        {
            try
            {
                var addresses = _customerAddressRepository.GetCustomerAddresses(customerId);
                return Ok(addresses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{addressId}")]
        public IActionResult GetCustomerAddress(int addressId)
        {
            try
            {
                var address = _customerAddressRepository.GetCustomerAddress(addressId);
                if (address == null)
                {
                    return NotFound();
                }
                return Ok(address);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult CreateCustomerAddress([FromBody] CustomerAddress customerAddress)
        {
            if (customerAddress == null)
            {
                return BadRequest("Customer address is null.");
            }

            try
            {
                bool created = _customerAddressRepository.CreateCustomerAddress(customerAddress);
                if (!created)
                {
                    return StatusCode(500, new { error = "Failed to create address." });
                }
                return Ok(new { message = "Address created successfully." });
            }
            catch (SqlException ex) when (ex.Number == 50001)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{addressId}")]
        public IActionResult UpdateCustomerAddress(int addressId, [FromBody] CustomerAddress customerAddress)
        {
            if (customerAddress == null)
            {
                return BadRequest("Customer address is null.");
            }

            try
            {
                bool updated = _customerAddressRepository.UpdateCustomerAddress(addressId, customerAddress);
                if (!updated)
                {
                    return NotFound(new { message = "Address not found." });
                }
                return Ok(new { message = "Address updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("{addressId}")]
        public IActionResult DeleteCustomerAddress(int addressId)
        {
            try
            {
                bool deleted = _customerAddressRepository.DeleteCustomerAddress(addressId);
                if (!deleted)
                {
                    return NotFound(new { message = "Address not found." });
                }
                return Ok(new { message = "Address deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
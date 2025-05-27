using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Interface;
using Ecommerce.Helper;
using System.Threading.Tasks;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ImageHelper _imageHelper;

        public CustomerController(ICustomerRepository customerRepository, ImageHelper imageHelper)
        {
            _customerRepository = customerRepository;
            _imageHelper = imageHelper;
        }

        [HttpGet("{customerId}")]
        public IActionResult GetCustomerProfile(int customerId)
        {
            try
            {
                var customer = _customerRepository.GetCustomerProfile(customerId);
                if (customer == null)
                {
                    return NotFound(new { message = "Customer not found." });
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("{customerId}/UploadImage")]
        public async Task<IActionResult> UploadCustomerImage(int customerId, IFormFile image)
        {
            try
            {
                string imageUrl = await _imageHelper.UploadImageAsync(image);
                bool updated = _customerRepository.UpdateCustomerImage(customerId, imageUrl);
                if (!updated)
                {
                    return StatusCode(500, new { message = "Failed to update profile image." });
                }
                return Ok(new { imageUrl });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Model;
using Ecommerce.Interface;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerSignupController : ControllerBase
    {
        private readonly ICustomerSignupRepository _customerSignupRepository;

        public CustomerSignupController(ICustomerSignupRepository customerSignupRepository)
        {
            _customerSignupRepository = customerSignupRepository;
        }

        [HttpPost("signup")]
        public IActionResult Signup([FromBody] CustomerSignupModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.CustomerName) || string.IsNullOrEmpty(model.Email) ||
                string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.ContactNumber))
            {
                return BadRequest(new { message = "Invalid signup details" });
            }

            try
            {
                bool success = _customerSignupRepository.SignupCustomer(model);
                if (success)
                {
                    return Ok(new { message = "Customer signup successful" });
                }
                else
                {
                    return BadRequest(new { message = "Failed to sign up customer" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
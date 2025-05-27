using Microsoft.AspNetCore.Mvc;
using Ecommerce.Model;
using Ecommerce.Interface;
using Ecommerce.Helper;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerLoginController : ControllerBase
    {
        private readonly ICustomerLoginRepository _customerLoginRepository;
        private readonly JwtHelper _jwtHelper;

        public CustomerLoginController(ICustomerLoginRepository customerLoginRepository, JwtHelper jwtHelper)
        {
            _customerLoginRepository = customerLoginRepository;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] CustomerLoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new { Message = "Invalid login credentials" });
            }

            try
            {
                var customer = _customerLoginRepository.GetCustomerByEmail(model.Email);
                if (customer == null)
                {
                    return NotFound(new { Message = "Email not found" });
                }

                if (PasswordHasher.VerifyPassword(model.Password, customer.PasswordHash))
                {
                    var token = _jwtHelper.GenerateCustomerJwtToken(customer.CustomerID);
                    return Ok(new CustomerLoginResponse
                    {
                        Message = "Login successful",
                        Token = token
                    });
                }
                else
                {
                    return Unauthorized(new { Message = "Invalid password" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
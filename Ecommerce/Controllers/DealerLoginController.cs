using Microsoft.AspNetCore.Mvc;
using Ecommerce.Model;
using Ecommerce.Interface;
using Ecommerce.Helper;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerLoginController : ControllerBase
    {
        private readonly IDealerLoginRepository _dealerLoginRepository;
        private readonly JwtHelper _jwtHelper;

        public DealerLoginController(IDealerLoginRepository dealerLoginRepository, JwtHelper jwtHelper)
        {
            _dealerLoginRepository = dealerLoginRepository;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] DealerLoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest(new { Message = "Invalid login credentials" });
            }

            try
            {
                var dealer = _dealerLoginRepository.GetDealerByEmail(model.Email);
                if (dealer == null)
                {
                    return NotFound(new { Message = "Email not found" });
                }

                if (PasswordHasher.VerifyPassword(model.Password, dealer.PasswordHash))
                {
                    var token = _jwtHelper.GenerateDealerJwtToken(dealer.DealerID);
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
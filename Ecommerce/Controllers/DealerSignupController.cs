using Ecommerce.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using static Ecommerce.Controllers.CustomerLoginController;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerSignupController : ControllerBase
    {
        private readonly IConfiguration _config;

        public DealerSignupController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("signup")]
        public IActionResult Signup([FromBody] DealerSignupModel model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid data" });
            }

            string? connectionString = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DealerSignup", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DealerName", model.DealerName);
                    cmd.Parameters.AddWithValue("@Email", model.Email);
                    cmd.Parameters.AddWithValue("@PasswordHash", Helper.PasswordHasher.HashPassword(model.Password));
                    cmd.Parameters.AddWithValue("@ContactNumber", (object?)model.ContactNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CompanyName", (object?)model.CompanyName ?? DBNull.Value);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        return Ok(new { message = "Dealer signup successful" });
                    }
                    catch (SqlException ex)
                    {
                        return BadRequest(new { message = ex.Message });
                    }
                }
            }
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var hashed = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashed);
        }
    }

}

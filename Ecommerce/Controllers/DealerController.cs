using Microsoft.AspNetCore.Mvc;
using Ecommerce.Model;
using Ecommerce.Interface;
using System.Collections.Generic;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerController : ControllerBase
    {
        private readonly IDealerRepository _dealerRepository;

        public DealerController(IDealerRepository dealerRepository)
        {
            _dealerRepository = dealerRepository;
        }

        [HttpGet]
        public IActionResult GetAllDealers()
        {
            try
            {
                var dealers = _dealerRepository.GetAllDealers();
                return Ok(dealers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{dealerId}")]
        public IActionResult GetDealerById(int dealerId)
        {
            try
            {
                var dealer = _dealerRepository.GetDealerById(dealerId);
                if (dealer == null)
                {
                    return NotFound(new { message = "Dealer not found." });
                }
                return Ok(dealer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Questlog.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        [HttpGet("details")]
        [Authorize]
        public IActionResult GetUserDetails()
        {
            // Retrieve the user ID from the claims (assuming it's stored in the "sub" claim or "nameidentifier")
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID not found.");
            }

            // Return the user ID or any other details you want
            return Ok(new { UserId = userId });
        }
    }
}
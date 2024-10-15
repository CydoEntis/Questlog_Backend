using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Questlog.Api.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    [HttpGet("details")]
    [Authorize]
    public IActionResult GetUserDetails()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized("User ID not found.");
        }

        return Ok(new { UserId = userId });
    }
}

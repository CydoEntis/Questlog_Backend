using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Services.Interfaces;

namespace Questlog.Api.Controllers;

[Route("api/avatars")]
[ApiController]
[Authorize]
[ServiceFilter(typeof(TokenValidationFilter))]
public class AvatarController : BaseController
{
    private readonly IAvatarService _avatarService;

    public AvatarController(IAvatarService avatarService)
    {
        _avatarService = avatarService;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<ApiResponse>> GetQuest(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            return BadRequestResponse("User Id must be provided");

        var result = await _avatarService.GetAvatarShop(userId);

        
        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }
}
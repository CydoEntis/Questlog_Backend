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

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllAvatars()
    {
        string userId = HttpContext.Items["UserId"] as string;

        if (string.IsNullOrEmpty(userId))
            return BadRequestResponse("User Id must be provided");


        var result = await _avatarService.GetAllAvatars(userId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

    [HttpGet("unlocked")]
    public async Task<ActionResult<ApiResponse>> GetUnlockedAvatars()
    {
        string userId = HttpContext.Items["UserId"] as string;

        if (string.IsNullOrEmpty(userId))
            return BadRequestResponse("User Id must be provided");


        var result = await _avatarService.GetUnlockedAvatars(userId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

    [HttpGet("next-tier")]
    public async Task<ActionResult<ApiResponse>> GetNextUnlockableTier()
    {
        string userId = HttpContext.Items["UserId"] as string;

        if (string.IsNullOrEmpty(userId))
            return BadRequestResponse("User Id must be provided");


        var result = await _avatarService.GetNextUnlockableTier(userId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }


    [HttpPost("unlock")]
    public async Task<ActionResult<ApiResponse>> UnlockAvatar(int avatarId)
    {
        string userId = HttpContext.Items["UserId"] as string;

        if (string.IsNullOrEmpty(userId))
            return BadRequestResponse("User Id must be provided");


        var result = await _avatarService.UnlockAvatar(userId, avatarId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }
}
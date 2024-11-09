using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Identity.Client;
using Questlog.Api.Models;
using Questlog.Application.Common.DTOs.Avatar;
using Questlog.Application.Services.Interfaces;

namespace Questlog.Api.Controllers;

[Route("api/user")]
[ApiController]
[Authorize]
[ServiceFilter(typeof(TokenValidationFilter))]
public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<ApiResponse>> GetQuest(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            return BadRequestResponse("User Id must be provided");

        var result = await _userService.GetUserById(userId);

        return !result.IsSuccess ? BadRequestResponse(result.ErrorMessage) : OkResponse(result.Data);
    }

    [HttpPut("avatar")]
    public async Task<ActionResult<ApiResponse>> UpdateAvatar([FromBody] UpdateAvatarDto dto)
    {
        string userId = HttpContext.Items["UserId"] as string;

        if (string.IsNullOrEmpty(userId))
            return BadRequestResponse("User Id must be provided");

        var result = await _userService.UpdateAvatar(userId, dto.id);

        return !result.IsSuccess ? BadRequestResponse(result.ErrorMessage) : OkResponse(result.Data);
    }
}
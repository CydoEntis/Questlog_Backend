using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.DTOs.Auth;
using Questlog.Application.Common.Exceptions;
using Questlog.Application.Services.Interfaces;
using Questlog.Application.Services.IServices;

namespace Questlog.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;

    public AuthController(IAuthService authService, ITokenService tokenService)
    {
        _authService = authService;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse>> Register([FromBody] RegisterDto registrationDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequestResponse("Invalid registration data.");
        }

        bool isUsernameUnique = await _authService.CheckIfUsernameIsUnique(registrationDto.Email);
        if (!isUsernameUnique)
        {
            var errorResponse = new Dictionary<string, string>
            {
                { "email", "Email is already in use." }
            };
            return BadRequest(errorResponse);
        }

        try
        {
            var result = await _authService.Register(registrationDto);
            if (result.IsSuccess)
            {
                return OkResponse(result.Data); 
            }

            return BadRequestResponse("Something went wrong while registering.");
        }
        catch (RegistrationException ex)
        {
            return BadRequestResponse(ex.InnerException.ToString());
        }
        catch (Exception ex)
        {
            return InternalServerErrorResponse("An unexpected error occurred.");
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse>> Login([FromBody] LoginCredentialsDto loginCredentialsDto)
    {
        var loginResponseDTO = await _authService.Login(loginCredentialsDto);
        if (!loginResponseDTO.IsSuccess)
        {
            return BadRequestResponse("Email or password is incorrect.");
        }

        return OkResponse(loginResponseDTO);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<ApiResponse>> GetNewAccessTokenFromRefreshToken([FromBody] TokenDTO tokenDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequestResponse("Invalid input.");
        }

        var newTokenDTO = await _tokenService.RefreshAccessToken(tokenDTO);
        if (newTokenDTO == null || string.IsNullOrEmpty(newTokenDTO.AccessToken))
        {
            return BadRequestResponse("Invalid token.");
        }

        return OkResponse(newTokenDTO);
    }

    [HttpPost("revoke")]
    public async Task<ActionResult<ApiResponse>> RevokeRefreshToken([FromBody] TokenDTO tokenDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequestResponse("Invalid input.");
        }

        await _tokenService.RevokeRefreshToken(tokenDTO);
        return OkResponse(new { message = "Refresh token revoked successfully." });
    }
}
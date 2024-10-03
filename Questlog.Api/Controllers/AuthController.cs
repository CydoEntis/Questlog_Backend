using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.DTOs.Auth;
using Questlog.Application.Common.Exceptions;
using Questlog.Application.Services.Interfaces;
using Questlog.Application.Services.IServices;
using System.Net;

namespace Questlog.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        protected ApiResponse _response;

        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _response = new();
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registrationRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    );

                return BadRequest(_response);
            }

            bool isUsernameUnique = await _authService.CheckIfUsernameIsUnique(registrationRequestDTO.Email);
            if (!isUsernameUnique)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Email", new List<string> { "Username is already taken" });
                return BadRequest(_response);
            }

            try
            {
                var user = await _authService.Register(registrationRequestDTO);

                if (user is null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Registration", new List<string> { "Something went wrong while registering." });
                    return BadRequest(_response);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = user;
                return Ok(_response);
            }
            catch (RegistrationException ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = ex.Errors;
                return BadRequest(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Unexpected", new List<string> { ex.Message });
                return BadRequest(_response);
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var loginResponseDTO = await _authService.Login(loginRequestDTO);
            if (loginResponseDTO.Tokens is null || string.IsNullOrEmpty(loginResponseDTO.Tokens.AccessToken))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Email", new List<string> { "Email or password is incorrect" });
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponseDTO;
            return Ok(_response);
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> GetNewAccessTokenFromRefreshToken([FromBody] TokenDTO tokenDTO)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Input", new List<string> { "Invalid input" });
                return BadRequest(_response);
            }

            var newTokenDTO = await _tokenService.RefreshAccessToken(tokenDTO);
            if (newTokenDTO == null || string.IsNullOrEmpty(newTokenDTO.AccessToken))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Token", new List<string> { "Invalid token" });
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = newTokenDTO;
            return Ok(_response);
        }


        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] TokenDTO tokenDTO)
        {
            if (!ModelState.IsValid)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Input", new List<string> { "Invalid Input" });
                return BadRequest(_response);
            }

            await _tokenService.RevokeRefreshToken(tokenDTO);
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }
    }
}

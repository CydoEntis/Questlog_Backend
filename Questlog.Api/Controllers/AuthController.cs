using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.DTOs;
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
            bool isUsernameUnique = await _authService.CheckIfUsernameIsUnique(registrationRequestDTO.Email);
            if (!isUsernameUnique)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username is already taken");
                return BadRequest(_response);
            }

            try
            {
                var user = await _authService.Register(registrationRequestDTO);

                if (user is null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Something went wrong while registering.");
                    return BadRequest(_response);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = user;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
                return Ok(_response);
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var tokenDTO = await _authService.Login(loginRequestDTO);
            if (tokenDTO is null || string.IsNullOrEmpty(tokenDTO.AccessToken))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password is incorrect");
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = tokenDTO;
            return Ok(_response);
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> GetNewAccessTokenFromRefreshToken([FromBody] TokenDTO tokenDTO)
        {
            if (!ModelState.IsValid)
            {
                _response.IsSuccess = false;
                _response.Result = "Invalid input";
                return BadRequest(_response);
            }

            var newTokenDTO = await _tokenService.RefreshAccessToken(tokenDTO);
            if (newTokenDTO is null || string.IsNullOrEmpty(newTokenDTO.AccessToken))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Invalid token");
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
                _response.Result = "Invalid Input";
                return BadRequest(_response);
            }

            await _tokenService.RevokeRefreshToken(tokenDTO);
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }
    }
}

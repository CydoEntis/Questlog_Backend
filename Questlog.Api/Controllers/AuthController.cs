using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Services.IServices;
using System.Net;

namespace Questlog.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ApiResponse _response;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
            _response = new();
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

    }
}

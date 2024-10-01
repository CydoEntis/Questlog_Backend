using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Services.Implementations;
using Questlog.Application.Services.Interfaces;
using System.Net;

namespace Questlog.Api.Controllers
{
    [Route("api/user-level")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(TokenValidationFilter))]
    public class UserLevelController : ControllerBase
    {
        protected ApiResponse _response;
        private readonly IUserLevelService _userLevelService;
        private readonly IMapper _mapper;

        public UserLevelController(IUserLevelService userLevelService, IMapper mapper)
        {
            _response = new ApiResponse();
            _userLevelService = userLevelService;
            _mapper = mapper;
        }

        [HttpPost("add-exp")]
        public async Task<ActionResult<ApiResponse>> AddExpToUser([FromBody] string questPriority)
        {
            string userId = HttpContext.Items["UserId"] as string;

            if (string.IsNullOrEmpty(userId))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("BadRequest", new List<string> { "User Id cannot be null." });
                return BadRequest(_response);
            }

            // Call the service to add experience based on the quest priority
            await _userLevelService.AddExpAsync(userId, questPriority);

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = "Experience points added successfully.";
            return Ok(_response);
        }

        [HttpPost("remove-exp")]
        public async Task<ActionResult<ApiResponse>> RemoveExpFromUser([FromBody] string questPriority)
        {
            string userId = HttpContext.Items["UserId"] as string;

            if (string.IsNullOrEmpty(userId))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("BadRequest", new List<string> { "User Id cannot be null." });
                return BadRequest(_response);
            }

            // Call the service to add experience based on the quest priority
            await _userLevelService.RemoveExpAsync(userId, questPriority);

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = "Experience points added successfully.";
            return Ok(_response);
        }


    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.DTOs.QuestBoard;
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


        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetQuestBoard()
        {
            string userId = HttpContext.Items["UserId"] as string;


            if (string.IsNullOrEmpty(userId))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("InvalidId", new List<string> { "User Id cannot be null" });
                return BadRequest(_response);
            }

            try
            {
                var userLevel = await _userLevelService.GetUserLevelAsync(userId);  
                //_response.Result = _mapper.Map<CreateQuestBoardRequestDTO>(questBoard);
                _response.Result = userLevel;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (KeyNotFoundException ex)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("NotFound", new List<string> { ex.Message });
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("ServerError", new List<string> { "An error occurred while retrieving the Quest Board." });
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
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

            try
            {
                await _userLevelService.AddExpAsync(userId, questPriority);

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = "Experience points added successfully.";
                return Ok(_response);
            }
            catch (ArgumentNullException ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("ArgumentNull", new List<string> { ex.Message });
                return BadRequest(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("ServerError", new List<string> { "An error occurred while adding exp." });
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }

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

            try
            {
                // Call the service to add experience based on the quest priority
                await _userLevelService.RemoveExpAsync(userId, questPriority);

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = "Experience points added successfully.";
                return Ok(_response);
            }
            catch (ArgumentNullException ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("ArgumentNull", new List<string> { ex.Message });
                return BadRequest(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("ServerError", new List<string> { "An error occurred while removing exp." });
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }


        }


    }
}

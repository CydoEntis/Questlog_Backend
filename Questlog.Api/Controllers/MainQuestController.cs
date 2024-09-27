
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.DTOs.MainQuest;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;
using System.Net;
using System.Threading.Tasks;

namespace Questlog.Api.Controllers
{
    [Route("api/main-quest")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(TokenValidationFilter))]
    public class MainQuestController : ControllerBase
    {
        protected ApiResponse _response;
        private readonly IMainQuestService _mainQuestService;
        private readonly IMapper _mapper;

        public MainQuestController(IMainQuestService mainQuestService, IMapper mapper)
        {
            _response = new();
            _mainQuestService = mainQuestService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllMainQuests()
        {
            // Get userId from HttpContext.Items set by TokenValidationFilter
            string userId = HttpContext.Items["UserId"] as string;

            try
            {
                var mainQuests = await _mainQuestService.GetAllMainQuestsForUser(userId);
                var mainQuestDtos = _mapper.Map<List<MainQuestResponseDTO>>(mainQuests);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = mainQuestDtos;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("ServerError", new List<string> { "An error occurred while retrieving the MainQuests." });
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        [HttpGet("{id:int}", Name = "GetMainQuest")]
        public async Task<ActionResult<ApiResponse>> GetMainQuest(int id)
        {
            if (id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("InvalidId", new List<string> { "ID must be greater than zero." });
                return BadRequest(_response);
            }

            string userId = HttpContext.Items["UserId"] as string;

            try
            {
                var mainQuest = await _mainQuestService.GetMainQuest(id, userId);
                _response.Result = _mapper.Map<CreateMainQuestRequestDTO>(mainQuest);
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
                _response.ErrorMessages.Add("ServerError", new List<string> { "An error occurred while retrieving the MainQuest." });
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateMainQuest([FromBody] CreateMainQuestRequestDTO createMainQuestDTO)
        {
            if (createMainQuestDTO == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("BadRequest", new List<string> { "CreateMainQuestRequestDTO cannot be null." });
                return BadRequest(_response);
            }

            var mainQuest = _mapper.Map<MainQuest>(createMainQuestDTO);
            string userId = HttpContext.Items["UserId"] as string;

            try
            {
                var newMainQuest = await _mainQuestService.CreateMainQuest(mainQuest, userId);
                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = new
                {
                    Message = $"Main Quest with id {newMainQuest.Id} was created successfully",
                    MainQuest = newMainQuest
                };
                return CreatedAtAction(nameof(GetMainQuest), new { id = newMainQuest.Id }, _response);
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
                _response.ErrorMessages.Add("ServerError", new List<string> { "An error occurred while creating the MainQuest." });
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        [HttpPut(Name = "UpdateMainQuest")]
        public async Task<ActionResult<ApiResponse>> UpdateMainQuest([FromBody] UpdateMainQuestRequestDTO updateMainQuestDTO)
        {
            if (updateMainQuestDTO == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("BadRequest", new List<string> { "UpdateMainQuestRequestDTO cannot be null." });
                return BadRequest(_response);
            }

            var mainQuest = _mapper.Map<MainQuest>(updateMainQuestDTO);
            string userId = HttpContext.Items["UserId"] as string;

            try
            {
                var updatedMainQuest = await _mainQuestService.UpdateMainQuest(mainQuest, userId);
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = updatedMainQuest;
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
                _response.ErrorMessages.Add("ServerError", new List<string> { "An error occurred while updating the MainQuest." });
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteMainQuest")]
        public async Task<ActionResult<ApiResponse>> DeleteMainQuest(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            string userId = HttpContext.Items["UserId"] as string;

            try
            {
                await _mainQuestService.DeleteMainQuest(id, userId);
                _response.StatusCode = HttpStatusCode.NoContent;
                return NoContent();
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
                _response.ErrorMessages.Add("ServerError", new List<string> { "An error occurred while updating the MainQuest." });
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }
    }
}

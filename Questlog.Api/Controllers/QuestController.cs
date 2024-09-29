using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.DTOs.MainQuest;
using Questlog.Application.Common.DTOs.Quest;
using Questlog.Application.Services.Implementations;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;
using System.Net;

namespace Questlog.Api.Controllers
{
    [Route("api/quest")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(TokenValidationFilter))]
    public class QuestController : ControllerBase
    {
        protected ApiResponse _response;
        private readonly IQuestService _questService;
        private readonly IMapper _mapper;

        public QuestController(IQuestService questService, IMapper mapper)
        {
            _response = new ApiResponse();
            _questService = questService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllQuests()
        {
            string userId = HttpContext.Items["UserId"] as string;

            try
            {
                var quests = await _questService.GetAllQuestsForUser(userId);
                var questDtos = _mapper.Map<List<QuestResponseDTO>>(quests);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = questDtos;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("ServerError", new List<string> { "An error occurred while retrieving the Quest s." });
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        [HttpGet("{id:int}", Name = "GetQuest")]
        public async Task<ActionResult<ApiResponse>> GetQuest(int id)
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
                var quest = await _questService.GetQuest(id, userId);
                _response.Result = _mapper.Map<CreateQuestRequestDTO>(quest);
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
                _response.ErrorMessages.Add("ServerError", new List<string> { "An error occurred while retrieving the Quest ." });
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateQuest([FromBody] CreateQuestRequestDTO createQuestDTO)
        {
            if (createQuestDTO == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("BadRequest", new List<string> { "CreateQuestRequestDTO cannot be null." });
                return BadRequest(_response);
            }

            var quest = _mapper.Map<Quest>(createQuestDTO);
            string userId = HttpContext.Items["UserId"] as string;

            try
            {
                int newQuestId = await _questService.CreateQuest(quest, userId);
                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = new
                {
                    Message = $"Quest with id {newQuestId} was created successfully",
                    Id = newQuestId
                };
                return CreatedAtAction(nameof(GetQuest), new { id = newQuestId }, _response);
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
                _response.ErrorMessages.Add("ServerError", new List<string> { "An error occurred while creating the Quest." });
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        [HttpPut(Name = "UpdateQuest")]
        public async Task<ActionResult<ApiResponse>> UpdateQuest([FromBody] UpdateQuestRequestDTO updateQuestDTO)
        {
            if (updateQuestDTO == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("BadRequest", new List<string> { "UpdateQuestRequestDTO cannot be null." });
                return BadRequest(_response);
            }

            var mainQuest = _mapper.Map<Quest>(updateQuestDTO);
            string userId = HttpContext.Items["UserId"] as string;

            try
            {
                var updatedQuest = await _questService.UpdateQuest(mainQuest, userId);
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = updatedQuest;
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
                _response.ErrorMessages.Add("ServerError", new List<string> { "An error occurred while updating the Quest." });
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteQuest")]
        public async Task<ActionResult<ApiResponse>> DeleteQuest(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            string userId = HttpContext.Items["UserId"] as string;

            try
            {
                await _questService.DeleteQuest(id, userId);
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
                _response.ErrorMessages.Add("ServerError", new List<string> { "An error occurred while updating the Quest." });
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }
    }
}

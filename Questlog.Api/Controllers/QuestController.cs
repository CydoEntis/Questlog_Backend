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
                _response.Errors.Add("ServerError", new List<string> { "An error occurred while retrieving the Quest s." });
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
                _response.Errors.Add("InvalidId", new List<string> { "ID must be greater than zero." });
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
                _response.Errors.Add("NotFound", new List<string> { ex.Message });
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.Errors.Add("ServerError", new List<string> { "An error occurred while retrieving the Quest ." });
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
                _response.Errors.Add("BadRequest", new List<string> { "CreateQuestRequestDTO cannot be null." });
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
                _response.Errors.Add("ArgumentNull", new List<string> { ex.Message });
                return BadRequest(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.Errors.Add("ServerError", new List<string> { "An error occurred while creating the Quest." });
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
                _response.Errors.Add("BadRequest", new List<string> { "UpdateQuestRequestDTO cannot be null." });
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
                _response.Errors.Add("NotFound", new List<string> { ex.Message });
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.Errors.Add("ServerError", new List<string> { "An error occurred while updating the Quest." });
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        [HttpPut("reorder", Name = "UpdateQuestOrder")]
        public async Task<ActionResult<ApiResponse>> UpdateQuestOrder([FromBody] List<UpdateQuestOrderRequestDTO> updateQuestOrderDTOs)
        {
            if (updateQuestOrderDTOs == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors.Add("BadRequest", new List<string> { "QuestBoard order data is invalid." });
                return BadRequest(_response);
            }

            string userId = HttpContext.Items["UserId"] as string;

            try
            {
                var quests = _mapper.Map<List<Quest>>(updateQuestOrderDTOs);
                await _questService.UpdateQuestsOrderInQuestBoard(quests, userId);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (KeyNotFoundException ex)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Errors.Add("NotFound", new List<string> { ex.Message });
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.Errors.Add("ServerError", new List<string> { "An error occurred while reordering the Quest Boards." });
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        [HttpPut("change-questboard", Name = "UpdateQuestsQuestBoard")]
        public async Task<ActionResult<ApiResponse>> UpdateQuestsQuestBoard([FromBody] List<UpdateQuestOrderRequestDTO> updateQuestOrderDTOs)
        {
            // Initialize response object
            var _response = new ApiResponse();

            if (updateQuestOrderDTOs == null || !updateQuestOrderDTOs.Any())
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors.Add("BadRequest", new List<string> { "Quest order data is invalid." });
                return BadRequest(_response);
            }

            // Retrieve the userId from the HttpContext (ensure that middleware properly sets it)
            string userId = HttpContext.Items["UserId"] as string;

            if (string.IsNullOrEmpty(userId))
            {
                _response.StatusCode = HttpStatusCode.Unauthorized;
                _response.IsSuccess = false;
                _response.Errors.Add("Unauthorized", new List<string> { "User is not authorized." });
                return Unauthorized(_response);
            }

            try
            {
                // Map the DTO to Quest entities
                var quests = _mapper.Map<List<Quest>>(updateQuestOrderDTOs);

                // Call service to update quest order in the quest boards
                var updatedQuests = await _questService.UpdateQuestsInQuestBoards(quests, userId);

                // Return a successful response
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = updatedQuests; // Return updated quests if needed
                return Ok(_response);
            }
            catch (KeyNotFoundException ex)
            {
                // Handle case when the quests were not found for the user
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Errors.Add("NotFound", new List<string> { ex.Message });
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                // General server error handling
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.Errors.Add("ServerError", new List<string> { "An error occurred while updating the quests." });
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
                _response.Errors.Add("NotFound", new List<string> { ex.Message });
                return NotFound(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.Errors.Add("ServerError", new List<string> { "An error occurred while updating the Quest." });
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }
    }
}

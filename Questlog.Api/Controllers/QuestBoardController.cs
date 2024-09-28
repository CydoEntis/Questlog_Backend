using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.DTOs.MainQuest;
using Questlog.Application.Common.DTOs.QuestBoard;
using Questlog.Application.Services.Implementations;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;
using System.Net;

namespace Questlog.Api.Controllers
{
    [Route("api/quest-board")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(TokenValidationFilter))]
    public class QuestBoardController : ControllerBase
    {
        protected ApiResponse _response;
        private readonly IQuestBoardService _questBoardService;
        private readonly IMapper _mapper;

        public QuestBoardController(IQuestBoardService questBoardService, IMapper mapper)
        {
            _response = new ApiResponse();
            _questBoardService = questBoardService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllQuestBoards([FromQuery] QuestBoardFilterParams filterParams)
        {
            string userId = HttpContext.Items["UserId"] as string;

            try
            {
                var questBoards = await _questBoardService.GetAllQuestBoardsForUser(filterParams, userId);
                var questBoardDtos = _mapper.Map<List<QuestBoardResponseDTO>>(questBoards);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = questBoardDtos;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("ServerError", new List<string> { "An error occurred while retrieving the Quest Boards." });
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }


        [HttpGet("{id:int}", Name = "GetQuestBoard")]
        public async Task<ActionResult<ApiResponse>> GetQuestBoard(int id)
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
                var questBoard = await _questBoardService.GetQuestBoard(id, userId);
                _response.Result = _mapper.Map<CreateQuestBoardRequestDTO>(questBoard);
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

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateQuestBoard([FromBody] CreateQuestBoardRequestDTO createQuestBoardDTO)
        {
            if (createQuestBoardDTO == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("BadRequest", new List<string> { "CreateQuestBoardRequestDTO cannot be null." });
                return BadRequest(_response);
            }

            var questBoard = _mapper.Map<QuestBoard>(createQuestBoardDTO);
            string userId = HttpContext.Items["UserId"] as string;

            try
            {
                int newQuestId = await _questBoardService.CreateQuestBoard(questBoard, userId);
                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = new
                {
                    Message = $"Quest Board with id {newQuestId} was created successfully",
                    Id = newQuestId
                };
                return CreatedAtAction(nameof(GetQuestBoard), new { id = newQuestId }, _response);
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
                _response.ErrorMessages.Add("ServerError", new List<string> { "An error occurred while creating the Quest Board." });
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        [HttpPut(Name = "UpdateQuestBoard")]
        public async Task<ActionResult<ApiResponse>> UpdateQuestBoard([FromBody] UpdateQuestBoardRequestDTO updateQuestBoardDTO)
        {
            if (updateQuestBoardDTO == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("BadRequest", new List<string> { "UpdateQuestBoardRequestDTO cannot be null." });
                return BadRequest(_response);
            }

            var mainQuest = _mapper.Map<QuestBoard>(updateQuestBoardDTO);
            string userId = HttpContext.Items["UserId"] as string;

            try
            {
                var updatedQuestBoard = await _questBoardService.UpdateQuestBoard(mainQuest, userId);
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = updatedQuestBoard;
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
                _response.ErrorMessages.Add("ServerError", new List<string> { "An error occurred while updating the Quest Board." });
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }


        [HttpPut("reorder", Name = "UpdateQuestBoardOrder")]
        public async Task<ActionResult<ApiResponse>> UpdateQuestBoardOrder([FromBody] List<UpdateQuestBoardOrderRequestDTO> updateOrderDTOs)
        {
            if (updateOrderDTOs == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("BadRequest", new List<string> { "QuestBoard order data is invalid." });
                return BadRequest(_response);
            }

            string userId = HttpContext.Items["UserId"] as string;

            try
            {
                var questBoards = _mapper.Map<List<QuestBoard>>(updateOrderDTOs);
                await _questBoardService.UpdateQuestBoardsOrder(questBoards, userId);
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
                _response.ErrorMessages.Add("ServerError", new List<string> { "An error occurred while reordering the Quest Boards." });
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }


        [HttpDelete("{id:int}", Name = "DeleteQuestBoard")]
        public async Task<ActionResult<ApiResponse>> DeleteQuestBoard(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            string userId = HttpContext.Items["UserId"] as string;

            try
            {
                await _questBoardService.DeleteQuestBoard(id, userId);
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
                _response.ErrorMessages.Add("ServerError", new List<string> { "An error occurred while updating the Quest Board." });
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }
    }
}

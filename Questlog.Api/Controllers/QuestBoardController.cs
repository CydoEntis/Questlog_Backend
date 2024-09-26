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
        public async Task<ActionResult<ApiResponse>> GetAllQuestBoards()
        {
            string userId = HttpContext.Items["UserId"] as string;

            try
            {
                var questBoards = await _questBoardService.GetAllQuestBoardsForUser(userId);
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
    }
}

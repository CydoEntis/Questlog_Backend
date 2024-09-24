using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;
using System.Net;

namespace Questlog.Api.Controllers
{
    [Route("api/main-quest")]
    [ApiController]
    [Authorize]
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

        [HttpGet("{id:int}", Name = "GetMainQuest")]
        public async Task<ActionResult<ApiResponse>> GetMainQuest(int id)
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var mainQuest = await _mainQuestService.GetMainQuest(id);

            if (mainQuest is null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            _response.Result = _mapper.Map<CreateMainQuestRequestDTO>(mainQuest);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateMainQuest([FromBody] CreateMainQuestRequestDTO createMainQuestDTO)
        {
            if (createMainQuestDTO == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }

            var mainQuest = _mapper.Map<MainQuest>(createMainQuestDTO);

            int newQuestId = await _mainQuestService.CreateMainQuest(mainQuest);


            _response.StatusCode = HttpStatusCode.Created;
            _response.Result = $"Main Quest with id {newQuestId} was created successfully";
            return Ok(_response);
        }

    }
}

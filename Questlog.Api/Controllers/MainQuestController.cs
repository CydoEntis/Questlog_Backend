using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Services.Interfaces;
using System.Net;

namespace Questlog.Api.Controllers
{
    [Route("api/main-quest")]
    [ApiController]
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

        [HttpGet("{id:string", Name = "GetMainQuest")]
        public async Task<ActionResult<ApiResponse>> GetMainQuest(string id)
        {
            if (id is null || string.IsNullOrEmpty(id))
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

            _response.Result = _mapper.Map<MainQuestDTO>(mainQuest);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
    }
}

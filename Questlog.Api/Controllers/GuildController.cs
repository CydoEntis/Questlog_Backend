using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.Constants;
using Questlog.Application.Common.DTOs.Guild;
using Questlog.Application.Common.DTOs.Quest;
using Questlog.Application.Services.Implementations;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;
using System.Net;

namespace Questlog.Api.Controllers
{
    [Route("api/guild")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(TokenValidationFilter))]
    public class GuildController : BaseController
    {

        protected ApiResponse _response;
        private readonly IGuildService _guildService;
        private readonly IGuildMemberService _guildMemberService;
        private readonly IPartyService _partyService;
        private readonly IPartyMemberService _partyMemberService;
        private readonly ICharacterService _characterService;
        private readonly IMapper _mapper;

        public GuildController(IGuildService guildService, IGuildMemberService guildMemberService, IPartyService partyService, IPartyMemberService partyMemberService, ICharacterService characterService, IMapper mapper)
        {
            _response = new ApiResponse();
            _guildService = guildService;
            _guildMemberService = guildMemberService;
            _partyService = partyService;
            _partyMemberService = partyMemberService;
            _characterService = characterService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateGuild([FromBody] CreateGuildRequestDTO requestDTO)
        {
            if (requestDTO == null)
            {
                return BadRequestResponse("CreateGuildRequestDTO cannot be null.");
            }

            string userId = HttpContext.Items["UserId"] as string;

            try
            {
                var createdGuildResponseDTO = await _guildService.CreateGuild(userId, requestDTO);
                return CreatedResponse(createdGuildResponseDTO);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequestResponse(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerErrorResponse("An error occurred while creating the Guild.");
            }
        }

    }
}

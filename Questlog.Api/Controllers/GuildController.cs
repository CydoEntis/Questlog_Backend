﻿using AutoMapper;
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
    public class GuildController : ControllerBase
    {

        protected ApiResponse _response;
        private readonly IGuildService _guildService;
        private readonly IPartyService _partyService;
        private readonly IPartyMemberService _partyMemberService;
        private readonly ICharacterService _characterService;
        private readonly IMapper _mapper;

        public GuildController(IGuildService guildService, IPartyService partyService, IPartyMemberService partyMemberService, ICharacterService characterService, IMapper mapper)
        {
            _response = new ApiResponse();
            _guildService = guildService;
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
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("BadRequest", new List<string> { "CreateQuestRequestDTO cannot be null." });
                return BadRequest(_response);
            }

            string userId = HttpContext.Items["UserId"] as string;
            Guild guild = _mapper.Map<Guild>(requestDTO);
            Party party = _mapper.Map<Party>(requestDTO.Party);
            try
            {
                Guild createdGuild = await _guildService.CreateGuild(userId, guild);

                Party newParty = new Party
                {
                    Name = party.Name,
                    GuildId = createdGuild.Id,
                };

                Party createdParty = await _partyService.CreateParty(userId, newParty);

                Character character = await _characterService.GetCharacterAsync(userId);

                PartyMember newPartyMember = new PartyMember
                {
                    PartyId = createdParty.Id,
                    Role = RoleConstants.GetRole(RoleConstants.Leader)
                };

                PartyMember createdPartyMember = await _partyMemberService.CreatePartyMember(userId, newPartyMember);

                //Party newParty = new Party
                //{
                //    Name = guild.Party.Name,
                //};

                //Party createdParty = await _partyService.CreateParty(userId, newParty);


                //GuildResponseDTO responseDTO = _mapper.Map<GuildResponseDTO>(newGuild);
                _response.StatusCode = HttpStatusCode.Created;
                //_response.Result = responseDTO;

                return _response;

                //return CreatedAtAction(nameof(GetQuest), new { id = newQuestId }, _response);
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
    }
}
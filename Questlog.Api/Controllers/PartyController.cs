using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.Constants;
using Questlog.Application.Common.DTOs.Guild;
using Questlog.Application.Common.DTOs.Party;
using Questlog.Application.Common.DTOs.PartyMember;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;
using System.Net;

namespace Questlog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartyController : BaseController
    {
        protected ApiResponse _response;
        private readonly IPartyService _partyService;
        private readonly IPartyMemberService _partyMemberService;
        private readonly IGuildMemberService _guildMemberService;
        private readonly IMapper _mapper;

        public PartyController(IPartyService partyService, IPartyMemberService partyMemberService, IGuildMemberService guildMemberService, IMapper mapper)
        {
            _response = new ApiResponse();
            _partyService = partyService;
            _partyMemberService = partyMemberService;
            _guildMemberService = guildMemberService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateParty([FromBody] CreatePartyRequestDTO requestDTO)
        {
            if (requestDTO == null)
            {
                return BadRequestResponse("CreatePartyRequestDTO cannot be null.");
            }

            string userId = HttpContext.Items["UserId"] as string;

            try
            {
                var party = _mapper.Map<Party>(requestDTO);
                Party createdParty = await _partyService.CreateParty(party);

                var foundGuildMember = await _guildMemberService.GetGuildMember(party.GuildId, userId);

                if(foundGuildMember is null)
                {
                    return BadRequestResponse("No guild member found");
                }

                PartyMember newPartyMember = new PartyMember
                {
                    UserId = userId,
                    PartyId = createdParty.Id,
                    GuildMemberId = foundGuildMember.Id,
                    Role = RoleConstants.PartyLeader,
                    JoinedAt = DateTime.UtcNow
                };

                PartyMember addedPartyMember = await _partyMemberService.CreatePartyMember(newPartyMember);

                var createPartyResponseDTO = _mapper.Map<CreatePartyResponseDTO>(createdParty);
                //createPartyResponseDTO.PartyMembers.Add(_mapper.Map<CreatePartyMemberResponseDTO>(addedPartyMember));

                return CreatedResponse(createPartyResponseDTO);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequestResponse(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerErrorResponse("An error occurred while creating the party.");
            }
        }

    }
}

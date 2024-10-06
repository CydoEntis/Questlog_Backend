using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.Constants;
using Questlog.Application.Common.DTOs.Guild;
using Questlog.Application.Common.DTOs.Party;
using Questlog.Application.Services.Interfaces;
using Questlog.Domain.Entities;
using System.Net;

namespace Questlog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartyController : ControllerBase
    {
        protected ApiResponse _response;
        private readonly IPartyService _partyService;
        private readonly IPartyMemberService _partyMemberService;
        private readonly IMapper _mapper;

        public PartyController(IPartyService partyService, IPartyMemberService partyMemberService, IMapper mapper)
        {
            _response = new ApiResponse();
            _partyService = partyService;
            _partyMemberService = partyMemberService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateParty([FromBody] CreatePartyRequestDTO requestDTO)
        {
            if (requestDTO == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("BadRequest", new List<string> { "CreateQuestRequestDTO cannot be null." });
                return BadRequest(_response);
            }

            string userId = HttpContext.Items["UserId"] as string;

            try
            {
                Party party = _mapper.Map<Party>(requestDTO);
                Party createdParty = await _partyService.CreateParty(party);

                PartyMember partyLeader = new PartyMember
                {
                    UserId = userId,
                    GuildId = party.GuildId,
                    Role = RoleConstants.GetRole(RoleConstants.PartyLeader),
                    PartyId = createdParty.Id
                };

                PartyMember createdPartyMember = await _partyMemberService.CreatePartyMember(partyLeader);

                CreatedPartyResponseDTO createdPartyResponseDTO = _mapper.Map<CreatedPartyResponseDTO>(createdParty);
                CreatedPartyMemberResponseDTO createdPartyMemberResponseDTO = _mapper.Map<CreatedPartyMemberResponseDTO>(partyLeader);
                createdPartyResponseDTO.PartyMembers = new List<CreatedPartyMemberResponseDTO> { createdPartyMemberResponseDTO };

                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = createdPartyMemberResponseDTO;

                return _response;
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

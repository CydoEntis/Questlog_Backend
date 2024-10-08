using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Questlog.Api.Models;
using Questlog.Application.Common.DTOs.PartyMember;
using Questlog.Application.Services.Interfaces;

namespace Questlog.Api.Controllers
{
    [Route("api/guilds/{guildId}/parties/{partyId}/members")]
    [Authorize]
    [ServiceFilter(typeof(TokenValidationFilter))]
    [ApiController]
    public class PartyMemberController : BaseController
    {
        protected ApiResponse _response;
        private readonly IPartyMemberService _partyMemberService;

        public PartyMemberController(IPartyMemberService partyMemberService
        {
            _response = new ApiResponse();
            _partyMemberService = partyMemberService;
        }

        [HttpGet("{partyMemberId}")]
        public async Task<ActionResult<ApiResponse>> GetPartyMember(int guildId, int partyId, int partyMemberId)
        {
            var result = await _partyMemberService.GetPartyMember(partyMemberId);
            if (!result.IsSuccess)
            {
                return BadRequestResponse(result.ErrorMessage);
            }

            return OkResponse(result.Data);
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllPartyMembers(int guildId, int partyId)
        {
            var result = await _partyMemberService.GetAllPartyMembers(partyId);
            if (!result.IsSuccess)
            {
                return BadRequestResponse(result.ErrorMessage);
            }

            return OkResponse(result.Data);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreatePartyMember(int guildId, int partyId, [FromBody] CreatePartyMemberRequestDTO requestDTO)
        {
            if (requestDTO == null)
            {
                return BadRequestResponse("Invalid party member data.");
            }

            var result = await _partyMemberService.CreatePartyMember(requestDTO);
            if (!result.IsSuccess)
            {
                return BadRequestResponse(result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetPartyMember), new { guildId, partyId, partyMemberId = result.Data.Id }, result.Data);
        }

        [HttpPut("{partyMemberId}")]
        public async Task<ActionResult<ApiResponse>> UpdatePartyMember(int guildId, int partyId, int partyMemberId, [FromBody] UpdatePartyMemberRequestDTO requestDTO)
        {
            if (requestDTO == null)
            {
                return BadRequestResponse("Invalid party member data.");
            }

            requestDTO.Id = partyMemberId;

            var result = await _partyMemberService.UpdatePartyMember(requestDTO);
            if (!result.IsSuccess)
            {
                return BadRequestResponse(result.ErrorMessage);
            }

            return OkResponse(result.Data);
        }

        [HttpDelete("{partyMemberId}")]
        public async Task<ActionResult<ApiResponse>> RemovePartyMember(int guildId, int partyId, int partyMemberId)
        {
            var result = await _partyMemberService.RemovePartyMember(partyMemberId);
            if (!result.IsSuccess)
            {
                return BadRequestResponse(result.ErrorMessage);
            }

            return OkResponse(result.Data);
        }
    }
}

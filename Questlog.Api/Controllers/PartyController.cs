using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.DTOs.Party;
using Questlog.Application.Services.Interfaces;

namespace Questlog.Api.Controllers;

[Route("api/guilds/{guildId}/parties")]
[Authorize]
[ServiceFilter(typeof(TokenValidationFilter))]
[ApiController]
public class PartyController : BaseController
{
    protected ApiResponse _response;
    private readonly IPartyService _partyService;

    public PartyController(IPartyService partyService)
    {
        _response = new ApiResponse();
        _partyService = partyService;
    }

    [HttpGet("{partyId}")]
    public async Task<ActionResult<ApiResponse>> GetPartyById(int guildId, int partyId)
    {
        var result = await _partyService.GetPartyById(guildId, partyId);
        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

    //[HttpGet]
    //public async Task<ActionResult<ApiResponse>> GetAllParties(int guildId)
    //{
    //    var result = await _partyService.GetAllParties(guildId);
    //    if (!result.IsSuccess)
    //    {
    //        return BadRequestResponse(result.ErrorMessage);
    //    }

    //    return OkResponse(result.Data);
    //}

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateParty(int guildId, [FromBody] CreatePartyRequestDTO requestDTO)
    {
        string userId = HttpContext.Items["UserId"] as string;

        requestDTO.GuildId = guildId;

        var result = await _partyService.CreateParty(userId, requestDTO, guildId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return CreatedAtAction(nameof(GetPartyById), new { guildId, partyId = result.Data.Id }, result.Data);
    }

    [HttpPut("{partyId}")]
    public async Task<ActionResult<ApiResponse>> UpdateParty(int guildId, int partyId, [FromBody] UpdatePartyRequestDTO requestDTO)
    {
        var result = await _partyService.UpdateParty(guildId, requestDTO);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

    [HttpDelete("{partyId}")]
    public async Task<ActionResult<ApiResponse>> DeleteParty(int guildId, int partyId)
    {
        var result = await _partyService.DeleteParty(guildId, partyId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }
}

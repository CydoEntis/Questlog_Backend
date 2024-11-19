using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.DTOs.Quest;
using Questlog.Application.Services.Interfaces;

namespace Questlog.Api.Controllers;

[Route("api/parties/{partyId}/quests")]
[ApiController]
[Authorize]
[ServiceFilter(typeof(TokenValidationFilter))]
public class QuestController : BaseController
{
    private readonly IQuestService _questService;

    public QuestController(IQuestService questService)
    {
        _questService = questService;
    }

    [HttpGet("{questId}")]
    public async Task<ActionResult<ApiResponse>> GetQuest(int partyId, int questId)
    {
        string userId = HttpContext.Items["UserId"] as string;

        if (questId <= 0) return BadRequestResponse("Quest Id must be provided.");

        var result = await _questService.GetQuestById(userId, partyId, questId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllQuests(int partyId, [FromQuery] QueryParamsDto queryParams)
    {
        string userId = HttpContext.Items["UserId"] as string;

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequestResponse("User Id is missing.");
        }


        var result = await _questService.GetAllQuests(partyId, userId, queryParams);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateQuest([FromBody] CreateQuestDto requestDto)
    {
        if (requestDto == null)
        {
            return BadRequestResponse("CreateQuestRequestDTO cannot be null.");
        }

        string userId = HttpContext.Items["UserId"] as string;

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequestResponse("User Id is missing.");
        }

        var result = await _questService.CreateQuest(userId, requestDto);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return CreatedResponse(result.Data);
    }

    [HttpPut("{questId:int}")]
    public async Task<ActionResult<ApiResponse>> UpdateQuestDetails(int questId,
        [FromBody] UpdateQuestDto requestDto)
    {
        if (requestDto == null)
        {
            return BadRequestResponse("UpdateQuestRequestDTO cannot be null.");
        }

        string userId = HttpContext.Items["UserId"] as string;

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequestResponse("User Id is missing.");
        }

        var result = await _questService.UpdateQuest(requestDto, userId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

    [HttpDelete("{questId}")]
    public async Task<ActionResult<ApiResponse>> DeleteQuest(int questId)
    {
        if (questId <= 0) return BadRequestResponse("Quest Id must be provided.");

        var result = await _questService.DeleteQuest(questId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }

    [HttpPost("{questId}/complete")]
    public async Task<ActionResult<ApiResponse>> CompleteQuest(int questId)
    {
        string userId = HttpContext.Items["UserId"] as string;

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequestResponse("User Id is missing.");
        }

        var result = await _questService.CompleteQuest(questId, userId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data); 
    }


    [HttpPut("{questId}/uncomplete")]
    public async Task<ActionResult<ApiResponse>> UncompleteQuest(int questId)
    {
        string userId = HttpContext.Items["UserId"] as string;

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequestResponse("User Id is missing.");
        }

        var result = await _questService.UncompleteQuest(questId, userId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }
}
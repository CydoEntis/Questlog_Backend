﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.DTOs.Quest.Request;
using Questlog.Application.Services.Interfaces;

namespace Questlog.Api.Controllers;

[Route("api/campaigns/{campaignId}/quests")]
[ApiController]
[Authorize]
[ServiceFilter(typeof(TokenValidationFilter))]
public class QuestController : BaseController
{
    protected ApiResponse _response;
    private readonly IQuestService _questService;

    public QuestController(IQuestService questService)
    {
        _response = new ApiResponse();
        _questService = questService;
    }

    // [HttpGet("{questId}")]
    // public async Task<ActionResult<ApiResponse>> GetQuest(int questId)
    // {
    //     if (questId <= 0) return BadRequestResponse("Quest Id must be provided.");
    //
    //     var result = await _questService.GetQuestById(questId);
    //
    //     if (!result.IsSuccess)
    //     {
    //         return BadRequestResponse(result.ErrorMessage);
    //     }
    //
    //     return OkResponse(result.Data);
    // }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetAllQuests(int campaignId, [FromQuery] QueryParamsDto queryParams)
    {
        string userId = HttpContext.Items["UserId"] as string;
    
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequestResponse("User Id is missing.");
        }
    
         
        var result = await _questService.GetAllQuests(campaignId, userId, queryParams);
    
        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }
    
        return OkResponse(result.Data);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateQuest([FromBody] CreateQuestRequestDto requestDto)
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

        //var createdQuestId = result.Data.Id; // Assuming Id is a property of QuestResponseDTO
        //var locationUri = Url.Action("GetQuest", "Quest", new { questId = createdQuestId }, HttpContext.Request.Scheme);

        //return CreatedResponse(new { Id = createdQuestId, Location = locationUri, Message = "Quest created successfully" });
    }

    // [HttpPut("{questId}/details")]
    // public async Task<ActionResult<ApiResponse>> UpdateQuestDetails(int questId, [FromBody] UpdateQuestDetailsRequestDto requestDto)
    // {
    //     if (requestDto == null)
    //     {
    //         return BadRequestResponse("UpdateQuestRequestDTO cannot be null.");
    //     }
    //
    //     string userId = HttpContext.Items["UserId"] as string;
    //
    //     if (string.IsNullOrEmpty(userId))
    //     {
    //         return BadRequestResponse("User Id is missing.");
    //     }
    //
    //     var result = await _questService.UpdateQuestDetails(requestDto, userId);
    //
    //     if (!result.IsSuccess)
    //     {
    //         return BadRequestResponse(result.ErrorMessage);
    //     }
    //
    //     return OkResponse(result.Data);
    // }

    // [HttpPut("{questId}/leader")]
    // public async Task<ActionResult<ApiResponse>> UpdateQuestLeader(int questId, [FromBody] UpdateQuestOwnerRequestDto requestDto)
    // {
    //     if (requestDto == null)
    //     {
    //         return BadRequestResponse("UpdateQuestRequestDTO cannot be null.");
    //     }
    //
    //     string userId = HttpContext.Items["UserId"] as string;
    //
    //     if (string.IsNullOrEmpty(userId))
    //     {
    //         return BadRequestResponse("User Id is missing.");
    //     }
    //
    //     var result = await _questService.UpdateQuestLeader(questId, userId, requestDto);
    //
    //     if (!result.IsSuccess)
    //     {
    //         return BadRequestResponse(result.ErrorMessage);
    //     }
    //
    //     return OkResponse(result.Data);
    // }

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
}
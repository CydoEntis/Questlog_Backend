using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Questlog.Api.Models;
using Questlog.Application.Common.DTOs.Step;
using Questlog.Application.Services.Interfaces;

namespace Questlog.Api.Controllers;

[Route("api/steps")]
[ApiController]
[Authorize]
[ServiceFilter(typeof(TokenValidationFilter))]
public class StepController : BaseController
{
    private readonly IStepService _stepService;

    public StepController(IStepService stepService)
    {
        _stepService = stepService;
    }

    [HttpPut]
    public async Task<ActionResult<ApiResponse>> UpdateQuestDetails(
        [FromBody] UpdateStepDto requestDto)
    {
        string userId = HttpContext.Items["UserId"] as string;

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequestResponse("User Id is missing.");
        }

        var result = await _stepService.UpdateStep(requestDto, userId);

        if (!result.IsSuccess)
        {
            return BadRequestResponse(result.ErrorMessage);
        }

        return OkResponse(result.Data);
    }
}
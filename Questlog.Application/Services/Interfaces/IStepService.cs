using Questlog.Application.Common.DTOs.Step;
using Questlog.Application.Common.Models;

namespace Questlog.Application.Services.Interfaces;

public interface IStepService
{
    Task<ServiceResult<StepDto>> UpdateStep(
        UpdateStepDto requestDto, string userId);
}
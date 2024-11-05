using AutoMapper;
using Microsoft.Extensions.Logging;
using Questlog.Application.Common.DTOs.Step;
using Questlog.Application.Common.Interfaces;
using Questlog.Application.Common.Models;
using Questlog.Application.Common.Validation;
using Questlog.Application.Services.Interfaces;

namespace Questlog.Application.Services.Implementations;

public class StepService : BaseService, IStepService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StepService(IUnitOfWork unitOfWork,
        ILogger<StepService> logger, IMapper mapper) : base(logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceResult<StepDto>> UpdateStep(UpdateStepDto requestDto, string userId)
    {
        try
        {
            var foundStep = await _unitOfWork.Step.GetAsync(s => s.Id == requestDto.Id);

            foundStep.UpdatedAt = DateTime.UtcNow;

            if (requestDto.IsCompleted.HasValue)
            {
                foundStep.IsCompleted = requestDto.IsCompleted.Value;
            }

            await _unitOfWork.Step.UpdateAsync(foundStep);

            var quest = await _unitOfWork.Quest.GetAsync(q => q.Id == foundStep.QuestId,
                includeProperties: "Steps");
            var completedStepsCount = quest.Steps.Count(s => s.IsCompleted);

            var responseDto = _mapper.Map<StepDto>(foundStep);
            responseDto.CompletedSteps = completedStepsCount;

            return ServiceResult<StepDto>.Success(responseDto);
        }
        catch (Exception ex)
        {
            return ServiceResult<StepDto>.Failure(ex.InnerException?.Message ?? ex.Message);
        }
    }
}
using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.DTOs.Quest;
using Questlog.Application.Common.DTOs.Quest.Request;
using Questlog.Application.Common.Models;

namespace Questlog.Application.Services.Interfaces;

public interface IQuestService
{
    Task<ServiceResult<GetQuestResponseDto>> GetQuestById(int campaignId, int questId);
    Task<ServiceResult<PaginatedResult<GetQuestResponseDto>>> GetAllQuests(int campaignId, string userId,
        QueryParamsDto queryParams);

    Task<ServiceResult<CreateQuestResponseDto>> CreateQuest(string userId,
        CreateQuestRequestDto requestDto);

    Task<ServiceResult<int>> DeleteQuest(int campaignId);
}
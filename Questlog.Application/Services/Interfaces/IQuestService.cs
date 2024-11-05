using Questlog.Application.Common.DTOs;
using Questlog.Application.Common.DTOs.Quest;
using Questlog.Application.Common.Models;

namespace Questlog.Application.Services.Interfaces;

public interface IQuestService
{
    Task<ServiceResult<QuestDto>> GetQuestById(int campaignId, int questId);

    Task<ServiceResult<PaginatedResult<QuestDto>>> GetAllQuests(int campaignId, string userId,
        QueryParamsDto queryParams);

    Task<ServiceResult<QuestDto>> CreateQuest(string userId,
        CreateQuestDto requestDto);

    Task<ServiceResult<QuestDto>> UpdateQuest(
        UpdateQuestDto requestDto, string userId);

    Task<ServiceResult<int>> DeleteQuest(int campaignId);

    Task<ServiceResult<QuestDto>> CompleteQuest(int questId, string userId);

    Task<ServiceResult<QuestDto>> UncompleteQuest(int questId, string userId);
}
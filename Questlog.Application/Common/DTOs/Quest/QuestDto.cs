﻿using Questlog.Application.Common.DTOs.Member;
using Questlog.Application.Common.DTOs.Step;

namespace Questlog.Application.Common.DTOs.Quest;

public class QuestDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Priority { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DueDate { get; set; }
    public int CampaignId { get; set; }
    public List<MemberDto> Members { get; set; }
    public List<StepDto> Steps { get; set; }
    public int CompletedSteps { get; set; }
}
﻿namespace Questlog.Application.Common.DTOs.Subquest.Response;

public record CreateTaskResponseDto
{
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
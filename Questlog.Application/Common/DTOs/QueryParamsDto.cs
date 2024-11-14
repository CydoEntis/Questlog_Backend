namespace Questlog.Application.Common.DTOs;

public record QueryParamsDto
{
    public string? Search { get; set; }
    public string? OrderBy { get; set; } = Enums.OrderBy.Desc.ToString();
    public string? Filter { get; set; } = "createdAt";
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 18;
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    
}
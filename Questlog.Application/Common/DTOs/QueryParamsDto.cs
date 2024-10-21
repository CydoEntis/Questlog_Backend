namespace Questlog.Application.Common.DTOs;

public record QueryParamsDto
{
    public string? SearchValue { get; set; }
    public string? OrderBy { get; set; } = Enums.OrderBy.Desc.ToString();
    public string? OrderOn { get; set; } = "createdAt";
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 18;
    
}
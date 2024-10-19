using Questlog.Application.Common.Enums;

namespace Questlog.Application.Common.DTOs.Campaign.Requests;

public record CampaignQueryParamsDto
{
    public string SortBy { get; set; } = Enums.SortBy.CreatedAt.ToString();
    public string OrderBy { get; set; } = Enums.OrderBy.Desc.ToString();
    public string? SearchBy { get; set; } = Enums.SearchBy.Name.ToString();
    public string? SearchValue { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 18;
    public DateTime? CreatedDateFrom { get; set; }
    public DateTime? CreatedDateTo { get; set; }
}

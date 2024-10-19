using Questlog.Application.Common.Enums;

namespace Questlog.Application.Common.DTOs.Campaign.Requests;

public record CampaignQueryParamsDto
{
    public string SortBy { get; set; } = SortByOptions.CreatedAt.ToString();
    public string OrderBy { get; set; } = OrderByOptions.Desc.ToString();
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 18;
    public DateTime? CreatedDateFrom { get; set; }
    public DateTime? CreatedDateTo { get; set; }
}

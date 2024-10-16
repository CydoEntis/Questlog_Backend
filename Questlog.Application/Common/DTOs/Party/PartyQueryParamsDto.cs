using Questlog.Application.Common.Enums;

namespace Questlog.Application.Common.DTOs.Party;

public record PartyQueryParamsDto
{
    public string SortBy { get; set; } = SortByOptions.CreatedAt.ToString();
    public string OrderBy { get; set; } = OrderByOptions.Desc.ToString();
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 2;
    public string? SearchBy { get; set; } = SearchByOptions.DisplayName.ToString();
    public string? SearchValue { get; set; }
    public DateTime? JoinDateFrom { get; set; }
    public DateTime? JoinDateTo { get; set; }
}
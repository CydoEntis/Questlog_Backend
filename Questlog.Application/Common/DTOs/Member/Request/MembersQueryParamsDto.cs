using Questlog.Application.Common.Enums;

namespace Questlog.Application.Common.DTOs.GuildMember.Request;

public record MembersQueryParamsDto
{
    public string SortBy { get; set; } = Enums.SortBy.JoinOn.ToString();
    public string OrderBy { get; set; } = Enums.OrderBy.Desc.ToString();
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 2;
    // public string? Role { get; set; }
    public string? SearchBy { get; set; } = Enums.SearchBy.DisplayName.ToString();
    public string? SearchValue { get; set; }
    public DateTime? JoinDateFrom { get; set; }
    public DateTime? JoinDateTo { get; set; }
}

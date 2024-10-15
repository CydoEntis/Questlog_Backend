using Questlog.Application.Common.Enums;

namespace Questlog.Application.Common.DTOs.GuildMember.Request;

public record GuildMembersQueryParamsDTO
{
    public string SortBy { get; set; } = SortByOptions.JoinOn.ToString();
    public string OrderBy { get; set; } = OrderByOptions.Desc.ToString();
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 2;
    public string? Role { get; set; }
    public DateTime? JoinDateFrom { get; set; }
    public DateTime? JoinDateTo { get; set; }
}

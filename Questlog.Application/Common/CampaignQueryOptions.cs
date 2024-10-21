using System.Linq.Expressions;
using Questlog.Application.Common.Enums;
using Questlog.Domain.Entities;

namespace Questlog.Application.Common;

public class CampaignQueryOptions
{
    public string? SearchValue { get; set; }
    public string? OrderBy { get; set; }
    public string? OrderOn { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? IncludeProperties { get; set; }
    public Expression<Func<Campaign, bool>> Filter { get; set; }

}
using System.Linq.Expressions;
using Questlog.Application.Common.Enums;
using Questlog.Domain.Entities;

namespace Questlog.Application.Common;

public class QueryOptions<T>
{
    public string? Search { get; set; }
    public string? OrderBy { get; set; }
    public string? SortBy { get; set; }
    public string? FilterDate { get; set; }
    public int? Priority { get; set; }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public string? IncludeProperties { get; set; }
    public Expression<Func<T, bool>> Filter { get; set; }

}
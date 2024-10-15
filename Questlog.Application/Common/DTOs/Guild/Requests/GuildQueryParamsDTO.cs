using Questlog.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.DTOs.Guild.Requests
{
    public record GuildQueryParamsDTO
    {
        public string SortBy { get; set; } = SortByOptions.CreatedAt.ToString();
        public string OrderBy { get; set; } = OrderByOptions.Desc.ToString();
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 2;
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
    }
}

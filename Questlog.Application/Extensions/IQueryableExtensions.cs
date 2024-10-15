using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplyRoleFilter<T>(
            this IQueryable<T> query,
            Expression<Func<T, string>> roleSelector,
            string? role)
        {
            if (!string.IsNullOrEmpty(role))
            {
                query = query.Where(e => roleSelector.Compile()(e) == role);
            }
            return query;
        }

        public static IQueryable<T> ApplyDateFilter<T>(
            this IQueryable<T> query,
            Expression<Func<T, DateTime>> dateSelector,
            DateTime? fromDate,
            DateTime? toDate)
        {
            if (fromDate.HasValue)
            {
                query = query.Where(e => dateSelector.Compile()(e) >= fromDate.Value);
            }
            if (toDate.HasValue)
            {
                query = query.Where(e => dateSelector.Compile()(e) <= toDate.Value);
            }
            return query;
        }
    }
}

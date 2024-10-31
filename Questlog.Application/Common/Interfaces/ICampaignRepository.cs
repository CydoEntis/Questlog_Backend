using System.Linq.Expressions;
using Questlog.Application.Common.Models;
using Questlog.Domain.Entities;

namespace Questlog.Application.Common.Interfaces;

public interface ICampaignRepository : IBaseRepository<Campaign>
{
    Task<PaginatedResult<Campaign>> GetPaginatedCampaignsAsync(
        QueryOptions<Campaign> queryOptions);

    Task<Campaign> UpdateAsync(Campaign entity);
}
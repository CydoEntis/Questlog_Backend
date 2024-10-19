using System.Linq.Expressions;
using Questlog.Domain.Entities;

namespace Questlog.Application.Common.Interfaces;

public interface ICampaignRepository : IBaseRepository<Campaign>
{
    Task<IEnumerable<Campaign>> GetAllAsync(CampaignQueryOptions options);
    // Task<IEnumerable<Campaign>> GetAll(Expression<Func<Campaign, bool>>? filter = null,
    //     string? includeProperties = null);
    Task<Campaign> UpdateAsync(Campaign entity);
}

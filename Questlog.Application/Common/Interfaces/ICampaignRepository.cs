using Questlog.Domain.Entities;

namespace Questlog.Application.Common.Interfaces;

public interface ICampaignRepository : IBaseRepository<Campaign>
{
    Task<Campaign> UpdateAsync(Campaign entity);
}

using Questlog.Application.Common.Interfaces;
using Questlog.Domain.Entities;
using Questlog.Infrastructure.Data;

namespace Questlog.Infrastructure.Repositories;

public class CampaignRepository : BaseRepository<Campaign>, ICampaignRepository
{
    private readonly ApplicationDbContext _db;

    public CampaignRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<Campaign> UpdateAsync(Campaign entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _db.Campaigns.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
}
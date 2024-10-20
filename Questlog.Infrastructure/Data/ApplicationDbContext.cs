using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Questlog.Domain.Entities;

namespace Questlog.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Campaign> Campaigns { get; set; }
    public DbSet<Member> Members { get; set; }


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Campaign>()
            .HasMany(c => c.Members)
            .WithOne(m => m.Campaign)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Campaign>()
            .HasOne(c => c.Owner)
            .WithMany()
            .HasForeignKey(c => c.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public void Seed()
{
    if (Users.Any())
    {
        return;
    }

    // Seed one admin user
    var user = new ApplicationUser
    {
        UserName = "Test",
        Email = "test@test.com",
        DisplayName = "Demo User",
        Avatar = Avatar.Archer,
        CurrentLevel = 99,
        CurrentExp = 999999,
        CreatedAt = DateTime.UtcNow
    };
    var passwordHasher = new PasswordHasher<ApplicationUser>();
    user.PasswordHash = passwordHasher.HashPassword(user, "Test123!");

    this.Users.Add(user);
    this.SaveChanges();

    var initialCampaign = new Campaign()
    {
        Name = "My First Campaign",
        Description = "The first campaign ever created",
        CreatedAt = DateTime.UtcNow,
        OwnerId = user.Id
    };
    this.Campaigns.Add(initialCampaign);
    this.SaveChanges();

    var initialMember = new Member
    {
        CampaignId = initialCampaign.Id,
        UserId = user.Id,
        Role = "Leader",
        JoinedOn = DateTime.UtcNow,
        UpdatedOn = DateTime.UtcNow
    };
    this.Members.Add(initialMember);
    this.SaveChanges();

    var random = new Random();
    List<ApplicationUser> usersList = new List<ApplicationUser>();

    string[] userDisplayNames = new string[]
    {
        "Elowen", "Kael", "Thalia", "Garrick", "Lysandra",
        "Roran", "Mira", "Zarek", "Selene", "Draven"
    };

    for (int i = 0; i < userDisplayNames.Length; i++)
    {
        var newUser = new ApplicationUser
        {
            UserName = userDisplayNames[i],
            Email = $"user{i + 1}@example.com",
            DisplayName = userDisplayNames[i],
            Avatar = new Avatar(),
            CurrentLevel = random.Next(1, 21),
            CurrentExp = random.Next(0, 1000),
            CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 30))
        };
        newUser.PasswordHash = passwordHasher.HashPassword(newUser, "Test123!");
        usersList.Add(newUser);
    }

    this.Users.AddRange(usersList);
    this.SaveChanges();

    string[] campaignNames = new string[]
    {
        "Guildmaster's Task", "Lost Artifacts Quest", "Merchant's Journey", 
        "Rescue Mission", "Heist of Legends", "Festival of Tasks", 
        "Siege Defense", "Knowledge Quest", "Bounty Ledger", 
        "Herbalist's Task", "Reclamation Efforts", "Arena Tournament", 
        "Construction Chronicles", "Expedition Quest", "Whispering Woods", 
        "Guild Expansion", "Siege Strategy", "Dragon's Deal", 
        "Alchemist's Help", "Elven Council", "Relics of Prophecy", 
        "Clockwork Secrets", "Monster Hunt", "Lost Library", 
        "Peace Initiatives"
    };

    string[] campaignDescriptions = new string[]
    {
        "Organize the guild's tasks.", "Locate and recover artifacts.", 
        "Manage a trading caravan.", "Rescue captives from dark forces.", 
        "Infiltrate a fortress for treasure.", "Organize a grand festival.", 
        "Defend a besieged fortress.", "Gather knowledge from libraries.", 
        "Track down dangerous bounties.", "Gather rare plants for potions.", 
        "Restore a cursed land.", "Organize a tournament event.", 
        "Rebuild a city after an attack.", "Lead an expedition into the tundra.", 
        "Uncover mysteries of the woods.", "Expand your guild's influence.", 
        "Plan an offensive against foes.", "Negotiate peace with a dragon.", 
        "Help an alchemist gather ingredients.", "Participate in the Elven Council.", 
        "Locate relics for a prophecy.", "Explore the Clockwork City.", 
        "Hunt dangerous creatures.", "Restore a forgotten library.", 
        "Negotiate peace treaties."
    };

    string[] campaignColors = new string[]
    {
        "blue", "green", "yellow", "red", "pink", 
        "purple", "orange", "amber", "navy", "gold"
    };

    List<Campaign> campaignsList = new List<Campaign>();
    for (int i = 0; i < campaignDescriptions.Length; i++)
    {
        var campaignEntity = new Campaign
        {
            Name = campaignNames[i],
            Description = campaignDescriptions[i],
            Color = campaignColors[random.Next(0, campaignColors.Length)],
            CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 30)),
            DueDate = DateTime.UtcNow.AddDays(random.Next(1, 30)),
            OwnerId = user.Id
        };
        campaignsList.Add(campaignEntity);
        this.Campaigns.Add(campaignEntity);
        this.SaveChanges(); 

 
        var campaignMember = new Member
        {
            CampaignId = campaignEntity.Id,
            UserId = user.Id,
            Role = "Leader",
            JoinedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };
        this.Members.Add(campaignMember);
    }

    this.SaveChanges();

    foreach (var campaign in campaignsList)
    {
        int membersCount = random.Next(1, 6);
        var membersToAdd = usersList.OrderBy(u => random.Next()).Take(membersCount).ToList();

        foreach (var member in membersToAdd)
        {
            var campaignMember = new Member
            {
                CampaignId = campaign.Id,
                UserId = member.Id,
                Role = "Member",
                JoinedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };
            this.Members.Add(campaignMember);
        }
    }

    this.SaveChanges();
}

}
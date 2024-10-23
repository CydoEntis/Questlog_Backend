using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Questlog.Domain.Entities;
using Task = Questlog.Domain.Entities.Task;

namespace Questlog.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Campaign> Campaigns { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Quest> Quests { get; set; }
    public DbSet<Task> Tasks { get; set; }


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

        modelBuilder.Entity<MemberQuest>()
            .HasKey(mq => new { mq.AssignedMemberId, mq.AssignedQuestId });

        modelBuilder.Entity<Quest>()
            .HasMany(q => q.AssignedMembers)
            .WithOne(mq => mq.AssignedQuest)
            .HasForeignKey(mq => mq.AssignedQuestId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Member>()
            .HasMany(m => m.MemberQuests)
            .WithOne(mq => mq.AssignedMember)
            .HasForeignKey(mq => mq.AssignedMemberId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Quest>()
            .HasMany(q => q.Tasks)
            .WithOne(s => s.Quest)
            .HasForeignKey(s => s.QuestId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public void Seed()
    {
        if (Users.Any())
        {
            return;
        }

        var user = SeedAdminUser();
        var campaigns = SeedInitialCampaign(user.Id);
        SeedRandomUsers();
        SeedRandomCampaigns(campaigns, user);
    }

    private ApplicationUser SeedAdminUser()
    {
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
        user.PasswordHash = HashPassword(user, "Test123*");

        Users.Add(user);
        SaveChanges();

        return user;
    }

    private Campaign SeedInitialCampaign(string ownerId)
    {
        var initialCampaign = new Campaign()
        {
            Name = "My First Campaign",
            Description = "The first campaign ever created",
            CreatedAt = DateTime.UtcNow,
            OwnerId = ownerId
        };
        Campaigns.Add(initialCampaign);
        SaveChanges();

        var initialMember = new Member
        {
            CampaignId = initialCampaign.Id,
            UserId = ownerId,
            Role = "Leader",
            JoinedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };
        Members.Add(initialMember);
        SaveChanges();

        return initialCampaign;
    }

    private void SeedRandomUsers()
    {
        var random = new Random();
        string[] userDisplayNames =
            { "Alex", "Jordan", "Taylor", "Casey", "Riley", "Morgan", "Skylar", "Jamie", "Cameron", "Avery" };
        var usersList = userDisplayNames.Select(name => new ApplicationUser
        {
            UserName = name,
            Email = $"{name.ToLower()}@example.com",
            DisplayName = name,
            Avatar = new Avatar(),
            CurrentLevel = random.Next(1, 21),
            CurrentExp = random.Next(0, 1000),
            CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 30)),
            PasswordHash = HashPassword(name, "Test123*")
        }).ToList();

        Users.AddRange(usersList);
        SaveChanges();
    }

    private void SeedRandomCampaigns(Campaign initialCampaign, ApplicationUser user)
    {
        var random = new Random();
        string[] campaignNames = { "Sprint Planning", "Bug Bash", "Code Review Marathon", /* ... */ };
        string[] campaignDescriptions =
            { "Organize tasks for the upcoming sprint.", "Fix bugs and improve stability.", /* ... */ };
        string[] campaignColors = { "red", "orange", "yellow", "green", "blue", "indigo", "violet" };

        foreach (var (name, description) in campaignNames.Zip(campaignDescriptions, (n, d) => (n, d)))
        {
            var campaign = new Campaign
            {
                Name = name,
                Description = description,
                Color = campaignColors[random.Next(campaignColors.Length)],
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 30)),
                DueDate = DateTime.UtcNow.AddDays(random.Next(1, 30)),
                OwnerId = user.Id
            };

            Campaigns.Add(campaign);
            SaveChanges();

            SeedMembers(campaign, user);
            SeedQuests(campaign);
        }
    }

    private void SeedMembers(Campaign campaign, ApplicationUser user)
    {
        var member = new Member
        {
            CampaignId = campaign.Id,
            UserId = user.Id,
            Role = "Leader",
            JoinedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };
        Members.Add(member);
    }

    private void SeedQuests(Campaign campaign)
    {
        var random = new Random();
        int questCount = random.Next(3, 21);

        for (int j = 0; j < questCount; j++)
        {
            var quest = new Quest
            {
                Name = $"Quest {j + 1} for {campaign.Name}",
                Description = $"Description for quest {j + 1} in {campaign.Name}",
                CampaignId = campaign.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 30))
            };
            Quests.Add(quest);
            SaveChanges();

            SeedTasks(quest);
        }
    }

    private void SeedTasks(Quest quest)
    {
        var random = new Random();
        int taskCount = random.Next(1, 6);
        bool allTasksCompleted = true;

        for (int k = 0; k < taskCount; k++)
        {
            bool isTaskCompleted = random.Next(0, 2) == 1; 
            if (!isTaskCompleted)
            {
                allTasksCompleted = false;
            }

            var task = new Task
            {
                Description = $"Task {k + 1} description for {quest.Name}",
                QuestId = quest.Id,
                IsCompleted = isTaskCompleted,
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 30))
            };
            Tasks.Add(task);
        }

        SaveChanges();

        if (allTasksCompleted)
        {
            quest.IsCompleted = true;
            SaveChanges();
        }
    }

    private string HashPassword(ApplicationUser user, string password)
    {
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        return passwordHasher.HashPassword(user, password);
    }
}
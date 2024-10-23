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

        var usersList = userDisplayNames.Select(name =>
        {
            var user = new ApplicationUser
            {
                UserName = name,
                Email = $"{name.ToLower()}@example.com",
                DisplayName = name,
                Avatar = new Avatar(),
                CurrentLevel = random.Next(1, 21),
                CurrentExp = random.Next(0, 1000),
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 30)),
            };

            // Hash the password for the created user
            user.PasswordHash = HashPassword(user, "Test123*"); // Pass the user instance instead of name

            return user;
        }).ToList();

        Users.AddRange(usersList);
        SaveChanges();
    }


    private void SeedRandomCampaigns(Campaign initialCampaign, ApplicationUser user)
    {
        var random = new Random();
        string[] campaignNames = new string[]
        {
            "Sprint Planning", "Bug Bash", "Code Review Marathon",
            "Feature Launch", "API Integration", "Frontend Overhaul",
            "Backend Optimization", "Database Migration", "Security Audit",
            "Performance Testing", "UI/UX Enhancement", "CI/CD Pipeline Setup",
            "Cloud Deployment", "Microservices Architecture", "Refactoring Sprint",
            "Agile Retrospective", "User Testing", "Code Freeze",
            "QA Automation", "Version Control Cleanup", "Team Onboarding",
            "Technical Debt Repayment", "Data Analytics Setup", "Documentation Week",
            "Innovation Sprint"
        };
        string[] campaignDescriptions = new string[]
        {
            "Organize tasks for the upcoming sprint.", "Fix bugs and improve stability.",
            "Perform in-depth code reviews across the team.", "Release new features to production.",
            "Integrate third-party APIs into the application.", "Revamp the frontend for a fresh look.",
            "Optimize backend performance and response times.", "Migrate data to a new database structure.",
            "Conduct a thorough security audit of the system.", "Test performance under load and stress.",
            "Improve the UI/UX based on user feedback.", "Set up automated CI/CD pipelines.",
            "Deploy the project to the cloud infrastructure.", "Implement microservices for scalability.",
            "Refactor code for better maintainability.", "Hold a retrospective on the last sprint.",
            "Conduct user testing sessions.", "Prepare for the code freeze before release.",
            "Automate quality assurance tests.", "Clean up and organize version control branches.",
            "Onboard new team members.", "Address and reduce technical debt.",
            "Set up data analytics for better insights.", "Dedicate time to writing and updating documentation.",
            "Explore new technologies in an innovation sprint."
        };
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
        var difficulties = new[] { "Easy", "Medium", "Hard" };
        int questCount = random.Next(3, 21);

        for (int j = 0; j < questCount; j++)
        {
            var quest = new Quest
            {
                Name = $"Quest {j + 1} for {campaign.Name}",
                Description = $"Description for quest {j + 1} in {campaign.Name}",
                CampaignId = campaign.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 30)),
                Difficulty = difficulties[random.Next(difficulties.Length)]
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Questlog.Application.Common.Constants;
using Questlog.Domain.Entities;

namespace Questlog.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Party> Parties { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Quest> Quests { get; set; }
    public DbSet<Step> Steps { get; set; }
    public DbSet<MemberQuest> MemberQuests { get; set; }
    public DbSet<InviteToken> InviteTokens { get; set; }
    public DbSet<Avatar> Avatars { get; set; }
    public DbSet<UnlockedAvatar> UnlockedAvatars { get; set; }


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MemberQuest>()
            .HasKey(mq => new { mq.AssignedMemberId, mq.AssignedQuestId });

        modelBuilder.Entity<MemberQuest>()
            .HasOne(mq => mq.AssignedMember)
            .WithMany(m => m.MemberQuests)
            .HasForeignKey(mq => mq.AssignedMemberId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MemberQuest>()
            .HasOne(mq => mq.AssignedQuest)
            .WithMany(q => q.MemberQuests)
            .HasForeignKey(mq => mq.AssignedQuestId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MemberQuest>()
            .HasOne(mq => mq.User)
            .WithMany()
            .HasForeignKey(mq => mq.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Member>()
            .HasOne(m => m.Party)
            .WithMany(c => c.Members)
            .HasForeignKey(m => m.PartyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Member>()
            .HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UnlockedAvatar>()
            .HasOne(ua => ua.User)
            .WithMany(u => u.UnlockedAvatars)
            .HasForeignKey(ua => ua.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UnlockedAvatar>()
            .HasOne(ua => ua.Avatar)
            .WithMany()
            .HasForeignKey(ua => ua.AvatarId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Avatar>().HasData(
            new Avatar { Id = 1, Name = "male_a", DisplayName = "Male Peasant", UnlockLevel = 1, Tier = 0, Cost = 0 },
            new Avatar { Id = 2, Name = "male_b", DisplayName = "Male Peasant", UnlockLevel = 1, Tier = 0, Cost = 0 },
            new Avatar { Id = 3, Name = "female_a", DisplayName = "Female Peasant", UnlockLevel = 1, Tier = 0, Cost = 0 },
            new Avatar { Id = 4, Name = "female_b", DisplayName = "Female Peasant", UnlockLevel = 1, Tier = 0, Cost = 0 },
            new Avatar { Id = 5, Name = "skeleton_a", DisplayName = "Skeleton Soldier", UnlockLevel = 3, Tier = 1, Cost = 200 },
            new Avatar { Id = 6, Name = "skeleton_b", DisplayName = "Skeleton Captain", UnlockLevel = 3, Tier = 1, Cost = 200 },
            new Avatar { Id = 7, Name = "zombie_male", DisplayName = "Male Zombie", UnlockLevel = 5, Tier = 2, Cost = 350 },
            new Avatar { Id = 8, Name = "zombie_female", DisplayName = "Female Zombie", UnlockLevel = 5, Tier = 2, Cost = 350 },
            new Avatar { Id = 9, Name = "bear", DisplayName = "Bear", UnlockLevel = 8, Tier = 3, Cost = 500 },
            new Avatar { Id = 10, Name = "gorilla", DisplayName = "Gorilla", UnlockLevel = 8, Tier = 3, Cost = 500 },
            new Avatar { Id = 11, Name = "frog", DisplayName = "Frog", UnlockLevel = 8, Tier = 3, Cost = 500 },
            new Avatar { Id = 12, Name = "snake", DisplayName = "Snake", UnlockLevel = 8, Tier = 3, Cost = 500 },
            new Avatar { Id = 13, Name = "medusa", DisplayName = "Medusa", UnlockLevel = 10, Tier = 4, Cost = 750 },
            new Avatar { Id = 14, Name = "knight", DisplayName = "Knight", UnlockLevel = 12, Tier = 5, Cost = 1000 },
            new Avatar { Id = 15, Name = "priest", DisplayName = "Priest", UnlockLevel = 12, Tier = 5, Cost = 1000 },
            new Avatar { Id = 16, Name = "mage", DisplayName = "Mage", UnlockLevel = 12, Tier = 5, Cost = 1000 },
            new Avatar { Id = 17, Name = "archer", DisplayName = "Archer", UnlockLevel = 12, Tier = 5, Cost = 1000 },
            new Avatar { Id = 18, Name = "rogue", DisplayName = "Rogue", UnlockLevel = 15, Tier = 5, Cost = 1200 },
            new Avatar { Id = 19, Name = "merfolk", DisplayName = "Merfolk", UnlockLevel = 16, Tier = 6, Cost = 1500 },
            new Avatar { Id = 20, Name = "squidman", DisplayName = "Squidman", UnlockLevel = 16, Tier = 6, Cost = 1500 },
            new Avatar { Id = 21, Name = "fishman", DisplayName = "Fishman", UnlockLevel = 16, Tier = 6, Cost = 1500 },
            new Avatar { Id = 22, Name = "mummy", DisplayName = "Mummy", UnlockLevel = 18, Tier = 7, Cost = 2000 },
            new Avatar { Id = 23, Name = "pharaoh", DisplayName = "Pharoah", UnlockLevel = 18, Tier = 7, Cost = 2000 },
            new Avatar { Id = 24, Name = "spider_a", DisplayName = "Spider", UnlockLevel = 18, Tier = 7, Cost = 2000 },
            new Avatar { Id = 25, Name = "fanatic", DisplayName = "Fanatic", UnlockLevel = 22, Tier = 8, Cost = 2500 },
            new Avatar { Id = 26, Name = "prince", DisplayName = "Prince", UnlockLevel = 22, Tier = 8, Cost = 2500 },
            new Avatar { Id = 27, Name = "occultist", DisplayName = "Occultist", UnlockLevel = 22, Tier = 8, Cost = 2500 },
            new Avatar { Id = 28, Name = "slime", DisplayName = "Slime", UnlockLevel = 28, Tier = 9, Cost = 3000 },
            new Avatar { Id = 29, Name = "mimic", DisplayName = "Mimic", UnlockLevel = 28, Tier = 9, Cost = 3000 },
            new Avatar { Id = 30, Name = "ghoul", DisplayName = "Ghoul", UnlockLevel = 28, Tier = 9, Cost = 3000 },
            new Avatar { Id = 31, Name = "goblin", DisplayName = "Goblin", UnlockLevel = 32, Tier = 10, Cost = 3500 },
            new Avatar { Id = 32, Name = "werewolf_a", DisplayName = "Werewolf Boss", UnlockLevel = 40, Tier = 11, Cost = 4000 },
            new Avatar { Id = 33, Name = "werewolf_b", DisplayName = "Werewolf Warrior", UnlockLevel = 40, Tier = 11, Cost = 4000 },
            new Avatar { Id = 34, Name = "werewolf_c", DisplayName = "Werewolf Chief", UnlockLevel = 40, Tier = 11, Cost = 4000 },
            new Avatar { Id = 35, Name = "male_orc", DisplayName = "Male Orc", UnlockLevel = 50, Tier = 12, Cost = 5000 },
            new Avatar { Id = 36, Name = "female_orc", DisplayName = "Female Orc", UnlockLevel = 50, Tier = 12, Cost = 5000 },
            new Avatar { Id = 37, Name = "lich", DisplayName = "Lich", UnlockLevel = 60, Tier = 13, Cost = 6000 },
            new Avatar { Id = 38, Name = "witch", DisplayName = "Witch", UnlockLevel = 70, Tier = 14, Cost = 7000 },
            new Avatar { Id = 39, Name = "angel", DisplayName = "Angel", UnlockLevel = 70, Tier = 14, Cost = 7000 },
            new Avatar { Id = 40, Name = "male_devil", DisplayName = "Male Devil", UnlockLevel = 80, Tier = 15, Cost = 8000 },
            new Avatar { Id = 41, Name = "female_devil", DisplayName = "Female Devil", UnlockLevel = 80, Tier = 15, Cost = 8000 },
            new Avatar { Id = 42, Name = "demon_male", DisplayName = "Male Demon", UnlockLevel = 100, Tier = 16, Cost = 10000 },
            new Avatar { Id = 43, Name = "demon_female", DisplayName = "Female Demon", UnlockLevel = 100, Tier = 16, Cost = 10000 }
        );
    }

    public void Seed()
    {
        if (Users.Any())
        {
            return;
        }

        var adminUser = SeedAdminUser();
        var parties = SeedInitialParty(adminUser.Id);
        SeedRandomUsers();
        SeedRandomParties(parties, adminUser);
    }

    private ApplicationUser SeedAdminUser()
    {
        var user = new ApplicationUser
        {
            UserName = "Test",
            Email = "test@test.com",
            DisplayName = "Demo User",
            AvatarId = 1,
            CurrentLevel = 3,
            CurrentExp = 300,
            CreatedAt = DateTime.UtcNow
        };
        user.PasswordHash = HashPassword(user, "Test123*");

        Users.Add(user);
        SaveChanges();

        var unlockedAvatars = Avatars.Where(a => a.UnlockLevel <= user.CurrentLevel).ToList();
        SeedUnlockedAvatars(user, unlockedAvatars); 

        return user;
    }


    private Party SeedInitialParty(string ownerId)
    {
        var initialParty = new Party()
        {
            Title = "My First Party",
            Description = "The first party ever created",
            CreatedAt = DateTime.UtcNow,
            OwnerId = ownerId
        };
        Parties.Add(initialParty);
        SaveChanges();

        var initialMember = new Member
        {
            PartyId = initialParty.Id,
            UserId = ownerId,
            Role = "Owner",
            JoinedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };
        Members.Add(initialMember);
        SaveChanges();

        return initialParty;
    }

    private void SeedRandomUsers()
    {
        var random = new Random();
        string[] userDisplayNames =
            { "Alex", "Jordan", "Taylor", "Casey", "Riley", "Morgan", "Skylar", "Jamie", "Cameron", "Avery" };

        var avatars = Avatars.ToList();

        var usersList = userDisplayNames.Select(name =>
        {
            var userLevel = random.Next(1, 101);

            var unlockedAvatars = avatars.Where(a => a.UnlockLevel <= userLevel).ToList();

            var assignedAvatar = unlockedAvatars[random.Next(unlockedAvatars.Count)];

            var currencyAmount = random.Next(100, 5001);

            int experienceForNextLevel = CalculateExpForLevel(userLevel);

            var user = new ApplicationUser
            {
                UserName = name,
                Email = $"{name.ToLower()}@example.com",
                DisplayName = name,
                AvatarId = assignedAvatar.Id,
                Currency = currencyAmount,
                CurrentLevel = userLevel,
                CurrentExp = random.Next(0, experienceForNextLevel),
                ExpToNextLevel = experienceForNextLevel,
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 30)),
            };

            user.PasswordHash = HashPassword(user, "Test123*");

            Users.Add(user);
            SaveChanges();

            SeedUnlockedAvatars(user, unlockedAvatars);

            return user;
        }).ToList();
    }

    private void SeedUnlockedAvatars(ApplicationUser user, List<Avatar> unlockedAvatars)
    {
        foreach (var avatar in unlockedAvatars)
        {
            var unlockedAvatar = new UnlockedAvatar
            {
                UserId = user.Id,
                AvatarId = avatar.Id,
                UnlockedAt = DateTime.UtcNow
            };
            UnlockedAvatars.Add(unlockedAvatar);
        }

        SaveChanges();
    }

    private int CalculateExpForLevel(int level)
    {
        int baseExp = 100;
        return baseExp * level;
    }


    private void SeedRandomParties(Party initialParty, ApplicationUser adminUser)
    {
        var random = new Random();
        string[] partyNames = new string[]
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
        string[] partyDescriptions = new string[]
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
        string[] partyColors = { "red", "orange", "yellow", "green", "blue", "indigo", "violet" };

        var users = ApplicationUsers.Where(u => u.Id != adminUser.Id).ToList();
        int partyCount = 0;

        foreach (var (title, description) in partyNames.Zip(partyDescriptions, (n, d) => (n, d)))
        {
            var party = new Party
            {
                Title = title,
                Description = description,
                Color = partyColors[random.Next(partyColors.Length)],
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 30)),
                DueDate = DateTime.UtcNow.AddDays(random.Next(1, 30)),
                OwnerId = partyCount < 5
                    ? users[random.Next(users.Count)].Id
                    : adminUser.Id
            };

            Parties.Add(party);
            SaveChanges();

            int numberOfMembers = random.Next(2, 9);
            var selectedMembers = users.OrderBy(u => random.Next()).Take(numberOfMembers).ToList();

            foreach (var memberUser in selectedMembers)
            {
                var role = memberUser.Id == party.OwnerId ? "Owner" : "Member";
                SeedMembers(party, memberUser, role);
            }

            if (party.OwnerId == adminUser.Id)
                SeedMembers(party, adminUser, "Owner");
            else
                SeedMembers(party, adminUser, "Member");

            SeedQuests(party);
            partyCount++;
        }
    }


    private void SeedMembers(Party party, ApplicationUser user, string role)
    {
        var member = new Member
        {
            PartyId = party.Id,
            UserId = user.Id,
            Role = role,
            JoinedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };
        Members.Add(member);
        SaveChanges();
    }

    private void SeedQuests(Party party)
    {
        var random = new Random();
        var difficulties = new[] { "Critical", "High", "Medium", "Low" };
        int questCount = random.Next(3, 21);

        var members = Members.Where(m => m.PartyId == party.Id).ToList();

        for (int j = 0; j < questCount; j++)
        {
            var quest = new Quest
            {
                Title = $"Quest {j + 1} for {party.Title}",
                Description = $"Description for quest {j + 1} in {party.Title}",
                PartyId = party.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 30)),
                Priority = difficulties[random.Next(difficulties.Length)]
            };
            Quests.Add(quest);
            SaveChanges();

            int numberOfQuestMembers = random.Next(1, 5);
            var questMembers = members.OrderBy(u => random.Next()).Take(numberOfQuestMembers).ToList();

            foreach (var member in questMembers)
            {
                var memberQuest = new MemberQuest
                {
                    AssignedMemberId = member.Id,
                    AssignedQuestId = quest.Id,
                    UserId = member.UserId
                };
                MemberQuests.Add(memberQuest);
            }

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

            var task = new Step
            {
                Description = $"Task {k + 1} description for {quest.Title}",
                QuestId = quest.Id,
                IsCompleted = isTaskCompleted,
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 30))
            };
            Steps.Add(task);
        }

        SaveChanges();

        if (allTasksCompleted)
        {
            quest.IsCompleted = true;
            quest.CompletionDate = DateTime.Now;

            int expReward = GetExpRewardForPriority(quest.Priority);
            int currencyReward = GetCurrencyRewardForPriority(quest.Priority);

            var memberQuests = MemberQuests.Where(mq => mq.AssignedQuestId == quest.Id).ToList();
            foreach (var memberQuest in memberQuests)
            {
                memberQuest.IsCompleted = true;
                memberQuest.AwardedExp = expReward;
                memberQuest.AwardedCurrency = currencyReward;

                var user = Users.FirstOrDefault(u => u.Id == memberQuest.UserId);
                if (user != null)
                {
                    user.CurrentExp += expReward;
                    user.Currency += currencyReward;
                    CheckForLevelUp(user);
                }
            }

            SaveChanges();
        }
    }

    private void CheckForLevelUp(ApplicationUser user)
    {
        while (user.CurrentExp >= user.ExpToNextLevel)
        {
            user.CurrentExp -= user.ExpToNextLevel;
            user.CurrentLevel++;
            user.ExpToNextLevel = CalculateExpForLevel(user.CurrentLevel);
        }
    }


    private int GetExpRewardForPriority(string priority)
    {
        return priority switch
        {
            "Critical" => LevelUpConstants.CriticalXPReward,
            "High" => LevelUpConstants.HighXPReward,
            "Medium" => LevelUpConstants.MediumXPReward,
            "Low" => LevelUpConstants.LowXPReward,
            _ => 0
        };
    }

    private int GetCurrencyRewardForPriority(string priority)
    {
        return priority switch
        {
            "Critical" => LevelUpConstants.CriticalCurrencyReward,
            "High" => LevelUpConstants.HighCurrencyReward,
            "Medium" => LevelUpConstants.MediumCurrencyReward,
            "Low" => LevelUpConstants.LowCurrencyReward,
            _ => 0
        };
    }


    private string HashPassword(ApplicationUser user, string password)
    {
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        return passwordHasher.HashPassword(user, password);
    }
}
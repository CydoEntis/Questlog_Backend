using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Questlog.Domain.Entities;

namespace Questlog.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<GuildMember> GuildMembers { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<PartyMember> PartyMembers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User deletion cascades to their character
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.GuildMembers)
                .WithOne(gm => gm.User)
                .OnDelete(DeleteBehavior.Cascade);

            // Prevent deletion of user if they are a guild leader
            modelBuilder.Entity<Guild>()
                .HasOne(g => g.GuildLeader)
                .WithMany()
                .HasForeignKey(g => g.GuildLeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            // When a guild is deleted, all its members and parties are deleted
            modelBuilder.Entity<Guild>()
                .HasMany(g => g.GuildMembers)
                .WithOne(gm => gm.Guild)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Guild>()
                .HasMany(g => g.Parties)
                .WithOne(p => p.Guild)
                .OnDelete(DeleteBehavior.Cascade);

            // When a party is deleted, restrict deletion of its party members
            modelBuilder.Entity<Party>()
                .HasMany(p => p.PartyMembers)
                .WithOne(pm => pm.Party)
                .OnDelete(DeleteBehavior.Restrict); // or NoAction

            // Adjust the relationship from PartyMember to Guild
            modelBuilder.Entity<PartyMember>()
                .HasOne(pm => pm.GuildMember)
                .WithMany() // This no longer needs to reference PartyMembers
                .OnDelete(DeleteBehavior.Restrict); // or NoAction

            // A PartyMember can be deleted independently without affecting other members
            modelBuilder.Entity<PartyMember>()
                .HasOne(pm => pm.Party)
                .WithMany(p => p.PartyMembers)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}

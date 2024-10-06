using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Questlog.Domain.Entities
{
    public class PartyMember
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; } // Foreign key to GuildMember

        // Foreign key to Guild (to relate to GuildMember)
        [ForeignKey("GuildMember")]
        public int GuildId { get; set; } // This references the Guild through GuildMember

        // Navigation property to GuildMember
        public GuildMember GuildMember { get; set; }

        [ForeignKey("Party")]
        public int PartyId { get; set; } // Foreign key to Party

        public Party Party { get; set; } // Navigation property to Party

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow; // Timestamp for when the member joined the party


        public IdentityRole Role { get; set; } 
    }

}

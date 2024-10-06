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
        public string UserId { get; set; } 

   
        [ForeignKey("GuildMember")]
        public int GuildId { get; set; } 

        public GuildMember GuildMember { get; set; }

        [ForeignKey("Party")]
        public int PartyId { get; set; } 

        public Party Party { get; set; } 

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow; 


        public IdentityRole Role { get; set; } 
    }

}

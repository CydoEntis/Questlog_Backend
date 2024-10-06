using Microsoft.AspNetCore.Identity;
using Questlog.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Questlog.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Character Character { get; set; }

        public virtual ICollection<Guild> Guilds { get; set; } = new List<Guild>();

        public virtual ICollection<GuildMember> GuildMembers { get; set; } = new List<GuildMember>();
    }
}

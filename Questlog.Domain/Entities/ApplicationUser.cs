using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Questlog.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Character Character { get; set; }

        // List of parties this user is part of
        public virtual List<PartyMember> JoinedParties { get; set; } = new List<PartyMember>();
    }
}

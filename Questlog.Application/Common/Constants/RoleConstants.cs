using Microsoft.AspNetCore.Identity;

namespace Questlog.Application.Common.Constants
{
    public static class RoleConstants
    {
        public const string GuildLeader = "GuildLeader"; // Can Do anything with in the guild 
        public const string GuildOfficer = "GuildOfficer"; // Can Invite and Remove Members and Create parties
        public const string GuildMember = "GuildMember"; // Can view the Guild

        public const string PartyCaptain = "PartyCaptain"; // Can Add/Remove Members From the Party and do everything else in the party
        public const string PartyStrategist = "PartyStrategist"; // Can Assign/Remove quests from party members
        public const string PartyMember = "PartyMember"; // Can view quests and complete quests assigned to them.



        // Method to get IdentityRole instances
        public static IdentityRole GetRole(string roleName)
        {
            return new IdentityRole(roleName);
        }
    }
}

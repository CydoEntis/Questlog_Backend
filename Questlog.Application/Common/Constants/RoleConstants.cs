using Microsoft.AspNetCore.Identity;

namespace Questlog.Application.Common.Constants
{
    public static class RoleConstants
    {
        public const string Leader = "Leader";
        public const string Strategist = "Strategist";
        public const string Member = "Member";

        // Method to get IdentityRole instances
        public static IdentityRole GetRole(string roleName)
        {
            return new IdentityRole(roleName);
        }
    }
}

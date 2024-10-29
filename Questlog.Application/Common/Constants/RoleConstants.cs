using Microsoft.AspNetCore.Identity;

namespace Questlog.Application.Common.Constants;

public static class RoleConstants
{
    public const string Owner = "Owner"; // Can Do anything with in the guild or Party
    public const string Captain = "Captain"; // Can Add/Remove Members From the Party/Guild and do everything else in the party
    public const string Strategist = "Strategist"; // Can Assign/Remove quests from party members
    public const string Member = "Member"; // Can view the Guild or Party



    // Method to get IdentityRole instances
    public static IdentityRole GetRole(string roleName)
    {
        return new IdentityRole(roleName);
    }
}

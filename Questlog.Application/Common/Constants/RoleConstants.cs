using Microsoft.AspNetCore.Identity;

namespace Questlog.Application.Common.Constants;

public static class RoleConstants
{
    public const string Creator = "creator";
    public const string Maintainer = "maintainer";
    public const string Member = "member"; 



    // Method to get IdentityRole instances
    public static IdentityRole GetRole(string roleName)
    {
        return new IdentityRole(roleName);
    }
}

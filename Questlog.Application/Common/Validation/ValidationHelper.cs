using Microsoft.AspNetCore.Identity;
using Questlog.Application.Common.Models;
using Questlog.Domain.Entities;

namespace Questlog.Application.Common.Validation;

public static class ValidationHelper
{
    public static async Task<ServiceResult> ValidateUserIdAsync(string userId, UserManager<ApplicationUser> userManager)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return ServiceResult.Failure("User does not exist.");
        }
        return ServiceResult.Success();
    }

    public static ServiceResult ValidateId(int id, string paramName)
    {
        if (id <= 0)
            return ServiceResult.Failure($"Must provide a valid {paramName}.");

        return ServiceResult.Success();
    }

    public static ServiceResult ValidateId(string id, string paramName)
    {
        if (string.IsNullOrEmpty(id))
            return ServiceResult.Failure($"Must provide a valid {paramName}.");

        return ServiceResult.Success();
    }

    public static ServiceResult ValidateObject<T>(T obj, string paramName) where T : class
    {
        if (obj == null)
            return ServiceResult.Failure($"Must provide a valid {paramName}.");

        return ServiceResult.Success();
    }
}





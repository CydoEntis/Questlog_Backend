using Questlog.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.Validation
{
    public static class ValidationHelper
    {
        public static ServiceResult ValidateUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return ServiceResult.Failure($"Must provide a valid User Id.");

            return ServiceResult.Success();
        }

        public static ServiceResult ValidateId(int id, string paramName)
        {
            if (id <= 0)
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
}





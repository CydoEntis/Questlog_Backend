using Microsoft.AspNetCore.Identity;
using Questlog.Application.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.Errors
{
    public class ErrorMapper
    {
        private readonly Dictionary<string, string> _errorCodeMappings = new()
        {
            {ErrorConstants.InvalidUserName, ErrorConstants.EmailMustOnlyContainLettersOrDigits }
        };

        public Dictionary<string, List<string>> Errors { get; set; }

        public ErrorMapper()
        {
            Errors = new();
        }

        public void MapErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                string errorMessage = GetMappedErrorMessage(error.Code, error.Description);

                if (error.Code == ErrorConstants.InvalidUserName)
                {
                    MapError(AppConstants.Email, errorMessage);
                }
                else if (error.Code == ErrorConstants.PasswordRequiresNonAlphanumeric)
                {
                    MapError(AppConstants.Password, errorMessage);
                }
            }

        }

        private void MapError(string key, string errorMessage)
        {
            if (!Errors.ContainsKey(key))
            {
                Errors[key] = new List<string>();
            }

            Errors[key].Add(errorMessage);
        }

        private string GetMappedErrorMessage(string errorCode, string errorDesc)
        {
            return _errorCodeMappings.TryGetValue(errorCode, out var message) ? message : errorDesc;
        }
    }
}

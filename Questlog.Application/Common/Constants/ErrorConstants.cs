using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Common.Constants
{
    public static class ErrorConstants
    {
        // Identifiers
        public const string InvalidUserName = "InvalidUserName";
        public const string PasswordRequiresNonAlphanumeric = "PasswordRequiresNonAlphanumeric";

        // Messages
        public const string EmailMustOnlyContainLettersOrDigits = "Email is invalid, can only contain letters or digits";
    }
}

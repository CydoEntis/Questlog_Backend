using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questlog.Application.Exceptions
{
    public class ServiceException : Exception
    {
        public string ErrorCode { get; }

        public ServiceException(string message, string errorCode = null) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}

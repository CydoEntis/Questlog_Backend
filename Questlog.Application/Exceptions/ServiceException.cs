namespace Questlog.Application.Exceptions;

public class ServiceException : Exception
{
    public string ErrorCode { get; }

    public ServiceException(string message, string errorCode = null) : base(message)
    {
        ErrorCode = errorCode;
    }
}

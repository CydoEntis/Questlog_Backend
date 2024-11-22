﻿namespace Questlog.Application.Common.Exceptions;

public class RegistrationException : Exception
{
    public Dictionary<string, List<string>> Errors { get; }
    public RegistrationException(Dictionary<string, List<string>> errors)
    : base("Incorrect Email Format")
    {
        Errors = errors;
    }
}

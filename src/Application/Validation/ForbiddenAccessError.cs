﻿namespace Application.Validation;

public class ForbiddenAccessError : Error
{
    public ForbiddenAccessError(string message) : base(message)
    {
    }
}

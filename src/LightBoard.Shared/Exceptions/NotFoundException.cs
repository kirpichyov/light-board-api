﻿namespace LightBoard.Shared.Exceptions;

public class NotFoundException : ExceptionBase
{
    public NotFoundException(string message) 
        : base(message, ExceptionIdentifiers.AppValidation)
    {
    }
}
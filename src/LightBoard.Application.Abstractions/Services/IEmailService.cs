﻿using LightBoard.Application.Abstractions.Arguments;

namespace LightBoard.Application.Abstractions.Services;

public interface IEmailService
{
    Task SendAsync(SendMailArgs args);
}
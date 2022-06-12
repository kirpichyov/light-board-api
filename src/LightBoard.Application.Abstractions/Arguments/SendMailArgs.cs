namespace LightBoard.Application.Abstractions.Arguments;

public record SendMailArgs(string To, string Subject, string Html);
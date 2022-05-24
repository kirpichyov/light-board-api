namespace LightBoard.Application.Abstractions.Services;

public interface IKeysGenerator
{
    string GenerateGuidBased();
    string Generate(int length);
}
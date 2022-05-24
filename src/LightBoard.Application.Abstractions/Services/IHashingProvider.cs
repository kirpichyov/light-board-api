namespace LightBoard.Application.Abstractions.Services;

public interface IHashingProvider
{
    string GetHash(string value);
    bool Verify(string value, string hash);
}
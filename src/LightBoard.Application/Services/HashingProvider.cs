using LightBoard.Application.Abstractions.Services;

namespace LightBoard.Application.Services;

public class HashingProvider : IHashingProvider
{
    public string GetHash(string value)
    {
        return BCrypt.Net.BCrypt.HashPassword(value);
    }

    public bool Verify(string value, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(value, hash);
    }
}
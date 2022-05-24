using System.Text;
using LightBoard.Application.Abstractions.Services;

namespace LightBoard.Application.Services;

public class KeysGenerator : IKeysGenerator
{
    private const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private const string Numbers = "0123456789";
    private const string Chars = Letters + Numbers;
    private readonly Random _random;

    public KeysGenerator()
    {
        _random = new Random();
    }

    public string GenerateGuidBased() => Guid.NewGuid().ToString().Replace('-', GetRandomChar());

    public string Generate(int length)
    {
        if (length <= 0)
        {
            throw new ArgumentException("Length can't be less or equal zero", nameof(length));
        }

        var stringBuilder = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            stringBuilder.Append(GetRandomChar());
        }

        return stringBuilder.ToString();
    }
        
    private char GetRandomChar() => Chars[_random.Next(maxValue: Chars.Length)];
}
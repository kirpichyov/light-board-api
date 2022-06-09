using Bogus;
using FluentAssertions;

namespace LightBoard.UnitTests;

public class SampleTests
{
    private readonly Faker _faker = new();
    
    [Fact]
    public void Test_ShouldBeEqualExpected()
    {
        // Act
        int initialValue = _faker.Random.Int(-500, 3500);
        int randomValue = _faker.Random.Int();
        long expected = initialValue + randomValue;

        // Act
        long result = initialValue + randomValue;

        // Assert
        result.Should().Be(expected);
    }
}
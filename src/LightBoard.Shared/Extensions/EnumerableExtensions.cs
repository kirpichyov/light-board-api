using System.Linq.Expressions;

namespace LightBoard.Shared.Extensions;

public static class EnumerableExtensions
{
    public static int MaxOrDefault<T> (this IEnumerable<T> numbers, Func<T, int> selector, int defaultValue = 1)
    {
        var numbersAsArray = numbers.Select(selector).ToArray();
        return numbersAsArray.Any() ? numbersAsArray.ToArray().Max() : defaultValue;
    }
}
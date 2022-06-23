namespace LightBoard.Domain.Contracts;

public interface IHasUniqueKey<TKey>
{
    TKey UniqueKey { get; }
}
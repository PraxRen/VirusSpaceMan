using System;

public interface IReadOnlyTrader
{
    event Action<ISimpleStorage> Changed;

    ISimpleStorage SimpleStorage { get; }
}
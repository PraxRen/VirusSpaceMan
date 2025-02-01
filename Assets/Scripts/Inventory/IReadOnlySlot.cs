using System;

public interface IReadOnlySlot<T> : ISimpleSlot where T : IObjectItem
{
    public T Item { get; }
}
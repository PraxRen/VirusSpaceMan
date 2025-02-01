using System;

public interface IReadOnlySlot<T> : ISimpleSlot where T : IObjectItem
{
    event Action<T, int> AddedItem;
    event Action<T, int> RemovedItem;
    
    public T Item { get; }
}
using System;

public interface IReadOnlySlot<T> where T : IObjectItem
{
    event Action<T, int> AddedItem;
    event Action<T, int> RemovedItem;
    
    public string Id { get; }
    public T Item { get; }
    public int Count { get; }
    public bool IsEmpty { get; }
}
using System;

public abstract class BaseSlot<T> : IReadOnlySlot<T> where T : IObjectItem
{
    protected BaseSlot() 
    {
        Id = Guid.NewGuid().ToString();
    }

    public string Id { get; }
    public T Item { get; protected set; }
    public int Count { get; protected set; }
    public bool IsEmpty => Item == null;

    public abstract bool TryAddItem(T item, int count);

    public abstract bool TryRemoveItem(int count);

    public abstract bool TryGiveItem(out T item, int count);
}
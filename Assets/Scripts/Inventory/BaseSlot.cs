using System;

public abstract class BaseSlot<T> : IReadOnlySlot<T> where T : IObjectItem
{
    public event Action AddedItem;
    public event Action RemovedItem;

    protected BaseSlot() 
    {
        Id = Guid.NewGuid().ToString();
    }

    public string Id { get; }
    public T Item { get; protected set; }
    public int Count { get; protected set; }
    public bool IsEmpty => Item == null;

    public bool TryAddItem(T item, int count)
    {
        bool result = TryAddItemAddon(item, count);

        if (result)
            AddedItem?.Invoke();

        return result;
    }

    public bool TryRemoveItem(int count)
    {
        bool result = TryRemoveItemAddon(count);

        if (result)
            RemovedItem?.Invoke();

        return result;
    }

    protected abstract bool TryAddItemAddon(T item, int count);

    protected abstract bool TryRemoveItemAddon(int count);

    public IObjectItem GetItem() => Item;
}
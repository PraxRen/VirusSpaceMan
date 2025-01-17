using System;

internal class Slot<T> : IReadOnlySlot<T> where T : Item
{
    public Slot(T item, int count)
    {
        AddItem(item, count);
    }

    public T Item { get; private set; }
    public int Count { get; private set; }
    public bool IsEmpty => Item == null;

    public void AddItem(T item, int count)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        if (Item != item)
            Count = 0;

        Item = item;
        Count += count;
    }

    public bool TryRemoveItem(int count)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        if (IsEmpty)
            return false;

        if (Count < count)
            return false;

        Count -= count;

        if (Count == 0)
            Item = null;

        return true;
    }

    public bool TryGiveItem(out T item, int count)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        item = null;

        if (IsEmpty)
            return false;

        if (TryRemoveItem(count) == false)
            return false;

        item = Item;

        return true;
    }

    public T GetItem() => Item;
}
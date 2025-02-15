using System;

public class Slot<T> : BaseSlot<T> where T : IObjectItem
{
    public Slot() : base() { }

    public Slot(T item, int count) : this()
    {
        if (TryAddItem(item, count) == false)
            throw new InvalidOperationException($"{nameof(BaseSlot<T>)} cannot be initialized");
    }

    protected override bool TryAddItemAddon(T item, int count)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        if (IsEmpty == false && Item.Id != item.Id)
            return false;

        if (Count >= item.LimitInSlot)
            return false;

        Item = item;
        Count += count;
        return true;
    }

    protected override bool TryRemoveItemAddon(int count)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        if (IsEmpty)
            return false;

        if (Count < count)
            return false;

        Count -= count;

        if (Count == 0)
            Item = default;

        return true;
    }
}
using System.Collections.Generic;
using System;
using System.Linq;

internal class Storage<T> : IReadOnlyStorage<T> where T : Item
{
    private readonly List<Slot<T>> _slots = new List<Slot<T>>();

    public Storage(int limitCapacity)
    {
        if (limitCapacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(limitCapacity));

        LimitCapacity = limitCapacity;
    }

    public int LimitCapacity { get; }
    public IReadOnlyCollection<IReadOnlySlot<T>> Slots => _slots;

    public bool TryAddItem(T item, int count)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        int capacity = GetCurrentCapacity() + count;

        if (capacity > LimitCapacity)
            return false;

        Slot<T> foundSlot = _slots.FirstOrDefault(slot => slot.Item == item);

        if (foundSlot == null)
            _slots.Add(new Slot<T>(item, count));
        else
            foundSlot.AddItem(item, count);

        return true;
    }

    public bool TryRemoveItem(T item, int count)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        Slot<T> foundSlot = _slots.FirstOrDefault(slot => slot.Item == item);

        if (foundSlot == null)
            return false;

        if (foundSlot.TryRemoveItem(count) == false)
            return false;

        if (foundSlot.IsEmpty)
            _slots.Remove(foundSlot);

        return true;
    }

    public bool TryGiveItem(out T item, int count, Func<Item, bool> predicate)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        item = null;
        Slot<T> slot = _slots.FirstOrDefault(slot => predicate.Invoke(slot.Item));

        if (slot == null)
            return false;

        if (slot.TryGiveItem(out item, count) == false)
            return false;

        return true;
    }

    public bool HasItem(T item) => _slots.Exists(slot => slot.Item == item);

    public int GetCurrentCapacity() => _slots.Sum(slot => slot.Count);
}
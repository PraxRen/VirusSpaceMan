using System.Collections.Generic;
using System;
using System.Linq;

public class Storage<T> : IStorage<T> where T : IObjectItem
{
    private readonly BaseSlot<T>[] _slots;

    public event Action<IReadOnlySlot<T>> ChangedSlot;

    public Storage(IEnumerable<BaseSlot<T>> slots)
    {
        if (slots == null || slots.Count() == 0)
            throw new ArgumentNullException(nameof(slots));

        BaseSlot<T>[] newSlots = slots.ToArray();

        if (newSlots.Length == 0)
            throw new ArgumentOutOfRangeException(nameof(slots));

        _slots = newSlots;
        LimitSlots = _slots.Length;
    }

    public int LimitSlots { get; }
    public IReadOnlyCollection<IReadOnlySlot<T>> Slots => _slots;

    public bool TryAddItem(IReadOnlySlot<T> slot, T item, int count)
    {
        if (slot == null)
            throw new ArgumentNullException(nameof(slot));

        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        BaseSlot<T> foundSlot = _slots.FirstOrDefault(insideSlot => slot.Id == insideSlot.Id);

        if (foundSlot == null)
            return false;

        if (foundSlot.TryAddItem(item, count) == false)
            return false;

        ChangedSlot?.Invoke(foundSlot);
        return true;
    }

    public bool TryAddItem(T item, int count)
    {
        BaseSlot<T> slot = _slots.FirstOrDefault(slot => slot.Item.Id == item.Id) ?? _slots.FirstOrDefault(slot => slot.IsEmpty);

        if (slot == null) 
            return false;

        return TryAddItem(slot, item, count);
    }

    public bool TryRemoveItem(IReadOnlySlot<T> slot, int count)
    {
        if (slot == null)
            throw new ArgumentNullException(nameof(slot));

        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        BaseSlot<T> foundSlot = _slots.FirstOrDefault(insideSlot => slot.Id == insideSlot.Id);

        if (foundSlot == null)
            return false;

        if (foundSlot.TryRemoveItem(count) == false)
            return false;

        ChangedSlot?.Invoke(foundSlot);
        return true;
    }

    public bool TryRemoveItem(T item, int count) 
    {
        BaseSlot<T> slot = _slots.FirstOrDefault(slot => slot.Item.Id == item.Id);
        
        if (slot == null)
            return false;

        return TryRemoveItem(slot, count);
    }

    public bool TryGiveItem(out T item, IReadOnlySlot<T> slot, int count)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        if (slot == null)
            throw new ArgumentNullException(nameof(slot));

        item = default;
        BaseSlot<T> foundSlot = _slots.FirstOrDefault(insideSlot => insideSlot.Id == slot.Id);
        
        if (foundSlot == null)
            return false;

        return foundSlot.TryGiveItem(out item, count);
    }

    public bool HasItem(T item) => _slots.Any(slot => slot.Item.Id == item.Id);
}
using System;
using System.Collections.Generic;

public interface IReadOnlyStorage<T> where T : IObjectItem
{
    public event Action<IReadOnlySlot<T>, T> AddedItem;
    public event Action<IReadOnlySlot<T>, T> RemovedItem;

    public int LimitSlots { get; }
    public IReadOnlyList<IReadOnlySlot<T>> Slots { get; }
}
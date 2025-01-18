using System;
using System.Collections.Generic;

public interface IReadOnlyStorage<T> where T : IObjectItem
{
    public event Action<IReadOnlySlot<T>> ChangedSlot;

    public int LimitSlots { get; }
    public IReadOnlyCollection<IReadOnlySlot<T>> Slots { get; }
}
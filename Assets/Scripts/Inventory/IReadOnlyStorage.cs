using System;
using System.Collections.Generic;

public interface IReadOnlyStorage<T> where T : IObjectItem
{
    event Action<IReadOnlySlot<T>, T> AddedItem;
    event Action<IReadOnlySlot<T>, T> RemovedItem;
    event Action Initialized;

    IReadOnlyList<IReadOnlySlot<T>> Slots { get; }
}
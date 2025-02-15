using System;
using System.Collections.Generic;

public interface IReadOnlyStorage<T> : ISimpleStorage where T : IObjectItem
{
    event Action<IReadOnlySlot<T>, T> AddedItem;
    event Action<IReadOnlySlot<T>, T> RemovedItem;

    IReadOnlyList<IReadOnlySlot<T>> Slots { get; }
}
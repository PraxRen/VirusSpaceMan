using System;

public interface IReadOnlyDisplayerStorage<T> where T : IObjectItem
{
    event Action<IReadOnlyDisplayerSlot<T>> ActiveDisplayerSlotChanged;

    IReadOnlyDisplayerSlot<T> ActiveDisplayerSlot { get; }
}
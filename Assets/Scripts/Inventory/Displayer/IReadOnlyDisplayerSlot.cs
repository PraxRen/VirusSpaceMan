using System;

public interface IReadOnlyDisplayerSlot<T> where T : IObjectItem
{
    public event Action<IDisplayerSlot<T>> Selected;

    public IReadOnlySlot<T> Slot { get; }
}
using System;

public interface IReadOnlyDisplayerSlot<T> where T : IObjectItem
{
    public event Action<IReadOnlyDisplayerSlot<T>> Selected;

    public IReadOnlySlot<T> Slot { get; }
}
using System;

public interface IReadOnlyDisplayerSlot
{
    public event Action<ISimpleSlot> Selected;

    public ISimpleSlot Slot { get; }
}
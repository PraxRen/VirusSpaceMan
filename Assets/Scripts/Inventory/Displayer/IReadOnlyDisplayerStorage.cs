using System;

public interface IReadOnlyDisplayerStorage
{
    event Action<IReadOnlyDisplayerSlot> ActiveDisplayerSlotChanged;

    IReadOnlyDisplayerSlot ActiveDisplayerSlot { get; }
}
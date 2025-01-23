using System;
using System.Collections.Generic;

public class DisplayerStorage<T> : IDisplayerStorage<T> where T : IObjectItem
{
    private List<IDisplayerSlot<T>> _displayerSlots = new List<IDisplayerSlot<T>>();
    private int _indexActiveSlot;

    public event Action<IReadOnlyDisplayerSlot<T>> ActiveDisplayerSlotChanged;

    public IReadOnlyDisplayerSlot<T> ActiveDisplayerSlot => _displayerSlots[_indexActiveSlot];

    public void Initilize(IReadOnlyStorage<T> storage, IDisplayerSlotFactory<T> displayerSlotFactory)
    {
        if (storage == null)
            throw new ArgumentNullException(nameof(storage));

        foreach (IReadOnlySlot<T> slot in storage.Slots)
        {
            _displayerSlots.Add(displayerSlotFactory.Create(slot, this));
        }

        ActiveDisplayerSlotChanged?.Invoke(ActiveDisplayerSlot);
    }

    public void Next()
    {
        _indexActiveSlot = _indexActiveSlot == _displayerSlots.Count - 1 ? 0 : _indexActiveSlot + 1;
        ActiveDisplayerSlotChanged?.Invoke(ActiveDisplayerSlot);
    }

    public void Previous()
    {
        _indexActiveSlot = _indexActiveSlot == 0 ? _displayerSlots.Count - 1 : _indexActiveSlot - 1;
        ActiveDisplayerSlotChanged?.Invoke(ActiveDisplayerSlot);
    }
}
using System;
using UnityEngine;

public abstract class DisplayerSlot3D<T> : MonoBehaviour, IDisplayerSlot<T> where T : IObjectItem
{
    public event Action<IReadOnlyDisplayerSlot<T>> Selected;

    public IReadOnlySlot<T> Slot { get; private set; }
    protected T Item { get; private set; }

    private void OnEnable()
    {
        if (Slot == null)
            return;

        Slot.AddedItem += OnAddedItem;
        Slot.RemovedItem += OnRemovedItem;
    }

    private void OnDisable()
    {
        ClearSlot();
    }

    public void Initialize(IReadOnlySlot<T> slot)
    {
        if (slot == null)
            throw new ArgumentNullException(nameof(slot));

        ClearSlot();
        Slot = slot;
        Slot.AddedItem += OnAddedItem;
        Slot.RemovedItem += OnRemovedItem;

        if (Slot.Item != null)
            InitializeItem(Slot.Item);
    }

    public abstract void Hide();

    public abstract void Show();

    protected abstract void InitializeItem(T item);

    protected abstract void ClearItem();

    private void ClearSlot()
    {
        if (Slot == null)
            return;

        Slot.AddedItem -= OnAddedItem;
        Slot.RemovedItem -= OnRemovedItem;
    }

    private void OnAddedItem(T item, int count)
    {
        if (Item != null)
            return;

        InitializeItem(item);
    }

    private void OnRemovedItem(T item, int count)
    {
        if (Item == null)
            return;

        if (item != null)
            return;

        ClearItem();
    }
}
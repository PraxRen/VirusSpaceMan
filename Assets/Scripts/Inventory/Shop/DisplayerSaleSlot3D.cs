using System;
using UnityEngine;

public class DisplayerSaleSlot3D : MonoBehaviour, IDisplayerSlot<ISaleItem>
{
    private Transform _transfrom;
    private ISaleItem _saleItem;
    private Graphics _graphics;

    public event Action<IDisplayerSlot<ISaleItem>> Selected;

    public IReadOnlySlot<ISaleItem> Slot { get; private set; }

    private void Awake()
    {
        _transfrom = transform;
    }

    private void OnEnable()
    {
        if (Slot == null)
            return;

        Slot.AddedItem += OnAddedItem;
        Slot.RemovedItem += OnRemovedItem;
    }

    private void OnDisable()
    {
        if (Slot == null)
            return;

        Slot.AddedItem -= OnAddedItem;
        Slot.RemovedItem -= OnRemovedItem;
    }

    public void Initilize(IReadOnlySlot<ISaleItem> slot)
    {
        if (slot == null)
            throw new ArgumentNullException(nameof(slot));

        if (Slot != null)
        {
            Slot.AddedItem -= OnAddedItem;
            Slot.RemovedItem -= OnRemovedItem;
        }

        Slot = slot;
        Slot.AddedItem += OnAddedItem;
        Slot.RemovedItem += OnRemovedItem;
        _saleItem = Slot.Item;

        if (_saleItem is IGraphicsItem graphicsItem)
        {
            Instantiate(graphicsItem.PrefabGraphics, _transfrom);
        }
    }

    private void OnAddedItem(ISaleItem saleItem, int count)
    {
        if (_saleItem == saleItem)
            return;
    }

    private void OnRemovedItem(ISaleItem saleItem, int count)
    {
        if (_saleItem == saleItem)
            return;
    }
}
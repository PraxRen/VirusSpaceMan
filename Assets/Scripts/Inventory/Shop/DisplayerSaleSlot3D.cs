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

    public void Initialize(IReadOnlySlot<ISaleItem> slot)
    {
        if (slot == null)
            throw new ArgumentNullException(nameof(slot));

        ClearSlot();
        Slot = slot;
        Slot.AddedItem += OnAddedItem;
        Slot.RemovedItem += OnRemovedItem;
        InitializeItem(Slot.Item);
    }

    private void InitializeItem(ISaleItem item)
    {
        if (item == null)
            return;

        _saleItem = Slot.Item;

        switch (_saleItem)
        {
            case IComplexWeaponConfig w:
                InitializeWeaponItem(w);
                break; 
        }
        
        if (_saleItem is IGraphicsItem graphicsItem)
        {
            _graphics = Instantiate(graphicsItem.PrefabGraphics, _transfrom);
            _graphics.Activate();
        }
    }

    private void InitializeWeaponItem(IComplexWeaponConfig complexWeapon)
    {

    }



    private void ClearSlot()
    {
        if (Slot == null)
            return;

        Slot.AddedItem -= OnAddedItem;
        Slot.RemovedItem -= OnRemovedItem;
        Slot = null;
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
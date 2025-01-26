using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDisplayerSaleSlotFactory", menuName = "Inventory/Shop/DisplayerSaleSlotFactory")]
public class DisplayerSaleSlotFactory : ScriptableObject, IDisplayerSlotFactory<ISaleItem>
{
    [SerializeField] private DisplayerSaleSlot3D _prefab;

    public IDisplayerSlot<ISaleItem> Create(IReadOnlySlot<ISaleItem> slot, IReadOnlyDisplayerStorage<ISaleItem> displayerStorage)
    {
        DisplayerShopStorage displayerShopStorage = displayerStorage as DisplayerShopStorage;

        if (displayerShopStorage == null)
            throw new ArgumentNullException(nameof(displayerShopStorage));

        DisplayerSaleSlot3D displayerSaleSlot3D = Instantiate(_prefab, displayerShopStorage.Transform);
        displayerSaleSlot3D.Initialize(slot);
        return displayerSaleSlot3D;
    }
}
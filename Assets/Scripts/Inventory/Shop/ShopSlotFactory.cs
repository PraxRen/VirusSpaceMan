using UnityEngine;

[CreateAssetMenu(fileName = "NewShopSlotFactory", menuName = "Inventory/Shop/ShopSlotFactory")]
public class ShopSlotFactory : ScriptableObject, ISlotFactory<ISaleItem>
{
    public BaseSlot<ISaleItem> Create(DataSlot dataSlot, IReadOnlyStorage<ISaleItem> storage)
    {
        return string.IsNullOrEmpty(dataSlot.IdItem) ? new Slot<ISaleItem>() : new Slot<ISaleItem>(Storage<ISaleItem>.FindItemInHash(dataSlot.IdItem), dataSlot.Count);
    }
}
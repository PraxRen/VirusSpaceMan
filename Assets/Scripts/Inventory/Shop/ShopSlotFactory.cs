using UnityEngine;

[CreateAssetMenu(fileName = "NewShopSlotFactory", menuName = "Inventory/Shop/ShopSlotFactory")]
public class ShopSlotFactory : ScriptableObject, ISlotFactory<ISaleItem>
{
    public BaseSlot<ISaleItem> Create(DataSlot dataSlot, IReadOnlyStorage<ISaleItem> storage)
    {
        return new Slot<ISaleItem>(Storage<ISaleItem>.FindItemInHash(dataSlot.IdItem), dataSlot.Count);
    }
}

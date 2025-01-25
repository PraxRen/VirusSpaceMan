using UnityEngine;

[CreateAssetMenu(fileName = "NewShopSlotFactory", menuName = "Inventory/Shop/ShopSlotFactory")]
public class ShopSlotFactory : ScriptableObject, ISlotFactory<ISaleItem>
{
    public BaseSlot<ISaleItem> Create(IDataSlot<ISaleItem> dataSlot, IReadOnlyStorage<ISaleItem> storage)
    {
        return new Slot<ISaleItem>(dataSlot.Item, dataSlot.Count);
    }
}

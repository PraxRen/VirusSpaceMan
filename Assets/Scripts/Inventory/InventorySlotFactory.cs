using UnityEngine;

[CreateAssetMenu(fileName = "NewInventorySlotFactory", menuName = "Inventory/InventorySlotFactory")]
public class InventorySlotFactory : ScriptableObject, ISlotFactory<IObjectItem>
{
    public BaseSlot<IObjectItem> Create(IDataSlot<IObjectItem> dataSlot, IReadOnlyStorage<IObjectItem> storage)
    {
        Slot<IObjectItem> slot = null;

        if (dataSlot.Item != null)
            slot = new Slot<IObjectItem>(dataSlot.Item, dataSlot.Count);
        else
            slot = new Slot<IObjectItem>();

        return slot;
    }
}
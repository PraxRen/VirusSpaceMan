using UnityEngine;

[CreateAssetMenu(fileName = "NewInventorySlotFactory", menuName = "Inventory/InventorySlotFactory")]
public class InventorySlotFactory : ScriptableObject, ISlotFactory<IObjectItem>
{
    public BaseSlot<IObjectItem> Create(DataSlot dataSlot, IReadOnlyStorage<IObjectItem> storage)
    {
        return string.IsNullOrEmpty(dataSlot.IdItem) ? new Slot<IObjectItem>() : new Slot<IObjectItem>(Storage<IObjectItem>.FindItemInHash(dataSlot.IdItem), dataSlot.Count);
    }
}
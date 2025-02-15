using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmentSlotFactory", menuName = "Inventory/Equipment/EquipmentSlotFactory")]
public class EquipmentSlotFactory : ScriptableObject, ISlotFactory<IEquipmentItem>
{
    public BaseSlot<IEquipmentItem> Create(DataSlot dataSlot, IReadOnlyStorage<IEquipmentItem> storage)
    {
        DataEquipmentSlot dataEquipmentSlot = dataSlot as DataEquipmentSlot;

        if (dataEquipmentSlot == null)
            throw new InvalidCastException(nameof(dataEquipmentSlot));

        return string.IsNullOrEmpty(dataSlot.IdItem) ? new EquipmentSlot(dataEquipmentSlot.Type) : new EquipmentSlot(Storage<IEquipmentItem>.FindItemInHash(dataSlot.IdItem), dataEquipmentSlot.Count);
    }
}
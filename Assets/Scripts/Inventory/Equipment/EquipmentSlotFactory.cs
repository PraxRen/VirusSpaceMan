using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmentSlotFactory", menuName = "Inventory/Equipment/EquipmentSlotFactory")]
public class EquipmentSlotFactory : ScriptableObject, ISlotFactory<IEquipmentItem>
{
    public BaseSlot<IEquipmentItem> Create(IDataSlot<IEquipmentItem> dataSlot, IReadOnlyStorage<IEquipmentItem> storage)
    {
        IDataEquipmentSlot dataEquipmentSlot = dataSlot as IDataEquipmentSlot;

        if (dataEquipmentSlot == null)
            throw new InvalidCastException(nameof(dataEquipmentSlot));

        return dataSlot.Item == null ? new EquipmentSlot(dataEquipmentSlot.Type) : new EquipmentSlot(dataEquipmentSlot.Item, dataEquipmentSlot.Count);
    }
}
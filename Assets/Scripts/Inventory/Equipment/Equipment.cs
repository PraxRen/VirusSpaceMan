using System;
using System.Collections.Generic;

public class Equipment : StorageMonoBehaviour<IEquipmentItem>
{
    protected override IEnumerable<BaseSlot<IEquipmentItem>> GetSlots()
    {
        foreach (EquipmentType type in Enum.GetValues(typeof(EquipmentType)))
        {
            yield return new EquipmentSlot(type);
        }
    }
}
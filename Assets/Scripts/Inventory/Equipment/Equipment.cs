using System;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : StorageMonoBehaviour<IEquipmentItem>
{
    [SerializeField] DefaultEquipmentSlots _default;

    protected override IEnumerable<BaseSlot<IEquipmentItem>> CreateSlots()
    {
        foreach (DefaultEquipmentSlots.SettingEquipmentSlot setting in _default.Slots)
        {
            EquipmentSlot equipmentSlot = new EquipmentSlot(setting.Item.Type);

            if (equipmentSlot.TryAddItem(setting.Item, 1) == false)
                throw new InvalidOperationException($"Error setting default values for {nameof(EquipmentSlot)}");

            yield return equipmentSlot;
        }
    }
}
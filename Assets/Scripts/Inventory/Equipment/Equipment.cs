using System;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : StorageMonoBehaviour<IEquipmentItem>
{
    [SerializeField] DefaultEquipmentSlots _default;

    protected override void StartAddon()
    {
        foreach (DefaultEquipmentSlots.SettingEquipmentSlot setting in _default.Slots)
        {
            if (TryAddItem(setting.Item, 1) == false)
                throw new InvalidOperationException($"Error setting default values for {nameof(EquipmentSlot)}");            
        }
    }

    protected override IEnumerable<BaseSlot<IEquipmentItem>> GetSlots()
    {
        foreach (EquipmentType type in Enum.GetValues(typeof(EquipmentType)))
        {
            yield return new EquipmentSlot(type);
        }
    }
}
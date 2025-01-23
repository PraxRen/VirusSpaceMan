using System.Collections.Generic;
using UnityEngine;

public class WeaponStorage : StorageMonoBehaviour<IComplexWeaponConfig>
{
    [SerializeField] private ShopStorageConfig _config;

    protected override IEnumerable<BaseSlot<IComplexWeaponConfig>> CreateSlots()
    {
        foreach (IComplexWeaponConfig item in _config.SaleItems)
        {
            yield return new Slot<IComplexWeaponConfig>(item, 1);
        }
    }
}
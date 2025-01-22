using System.Collections.Generic;
using UnityEngine;

public class ShopStorage : StorageMonoBehaviour<ISaleItem>
{
    [SerializeField] private ShopStorageConfig _config;

    protected override IEnumerable<BaseSlot<ISaleItem>> CreateSlots()
    {
        foreach (ISaleItem item in _config.SaleItems)
        {
            Slot<ISaleItem> slot = new Slot<ISaleItem>(item, 1);
            yield return slot;
        }
    }
}
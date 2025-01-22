using System.Collections.Generic;
using UnityEngine;

public class Shop : StorageMonoBehaviour<ISaleItem>
{
    [SerializeField] private ShopConfig _config;

    protected override IEnumerable<BaseSlot<ISaleItem>> GetSlots()
    {
        foreach (ISaleItem item in _config.SaleItems)
        {
            yield return new Slot<ISaleItem>(item, 1);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class Inventory : StorageMonoBehaviour<IObjectItem>
{
    [SerializeField] private int _countSlot;

    protected override IEnumerable<BaseSlot<IObjectItem>> CreateSlots()
    {
        for (int i = 0; i < _countSlot; i++)
        {
            yield return new Slot<IObjectItem>();
        }
    }
}
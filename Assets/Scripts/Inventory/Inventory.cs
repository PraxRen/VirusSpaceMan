using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class Inventory : StorageMonoBehaviour<IObjectItem>
{
    [SerializeField] private int _countSlot;

    protected override IEnumerable<BaseSlot<IObjectItem>> GetSlots()
    {
        for (int i = 0; i < _countSlot; i++)
        {
            yield return new Slot<IObjectItem>();
        }
    }
}
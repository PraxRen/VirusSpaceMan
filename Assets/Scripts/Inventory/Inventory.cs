using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class Inventory : StorageMonoBehaviour<IObjectItem>
{
    [SerializeField] private int _countSlot;
#if UNITY_EDITOR
    [SerializeField][ReadOnly] private List<string> _idsItems;
#endif

    protected override IEnumerable<BaseSlot<IObjectItem>> GetSlots()
    {
        for (int i = 0; i < _countSlot; i++)
        {
            yield return new Slot<IObjectItem>();
        }
    }
#if UNITY_EDITOR
    protected override void HandleAddedItem(IReadOnlySlot<IObjectItem> slot, IObjectItem item) => _idsItems.Add(item.Id);

    protected override void HandlenRemovedItem(IReadOnlySlot<IObjectItem> slot, IObjectItem item) => _idsItems.Remove(item.Id);
#endif
}
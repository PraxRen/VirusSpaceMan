using System;
using UnityEngine;

public class Buyer : Trader<IObjectItem>
{
    [SerializeField] private Equipment _equipment;

    public void BuyItem(ISaleItem item, int count)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (Storage.TryAddItem(item, count) == false)
            throw new InvalidOperationException("The buyer couldn't add the item");
    }

    public void Equip(IEquipmentItem equipmentItem)
    {
        if (_equipment.HasItem(equipmentItem, out _))
            throw new InvalidOperationException("This item is already equipped.");

        if (Storage.HasItem(equipmentItem, out _) == false)
            throw new InvalidOperationException("The buyer does not have this item in the storage.");

        if (Storage.TryRemoveItem(equipmentItem, 1) == false)
            throw new InvalidOperationException("The buyer couldn't remove the item");

        HandleOldEquipmentItem(equipmentItem);

        if (_equipment.TryAddItem(equipmentItem, 1) == false)
            throw new InvalidOperationException("The buyer couldn't add the EquipmentItem");
    }

    public bool HasItemInStorage(string idItem)
    {
        if (string.IsNullOrEmpty(idItem))
            throw new ArgumentNullException(nameof(idItem));

        return Storage.HasItem(idItem, out _);
    }

    public bool HasItemInEquipment(string idItem)
    {
        if (string.IsNullOrEmpty(idItem))
            throw new ArgumentNullException(nameof(idItem));

        return _equipment.HasItem(idItem, out _);
    }

    private void HandleOldEquipmentItem(IEquipmentItem newEquipmentItem)
    {
        Func<IReadOnlySlot<IEquipmentItem>, bool> predicate = (baseSlot) =>
        {
            EquipmentSlot equipmentSlot = baseSlot as EquipmentSlot;

            if (equipmentSlot == null)
                return false;

            return equipmentSlot.Type == newEquipmentItem.Type && equipmentSlot.IsEmpty == false;
        };

        if (_equipment.TryFindSlot(out ISimpleSlot slot, predicate) == false)
            return;

        if (_equipment.TryGiveItem(out IEquipmentItem oldItem, slot, 1) == false)
            return;

        if (Storage.TryAddItem(oldItem, 1) == false)
            throw new InvalidOperationException("The buyer couldn't add the item");
    }
}
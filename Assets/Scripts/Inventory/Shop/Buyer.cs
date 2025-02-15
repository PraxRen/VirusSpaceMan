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
        if (Storage.TryRemoveItem(equipmentItem, 1))
            throw new InvalidOperationException("The buyer couldn't remove the item");

        if (_equipment.TryAddItem(equipmentItem, 1))
            throw new InvalidOperationException("The buyer couldn't remove the item");
    }
}
using System;

public class Seller : Trader<ISaleItem>
{
    public ISaleItem SellItem(ISimpleSlot slot, int count)
    {
        if (slot == null)
            throw new ArgumentNullException(nameof(slot));

        ISaleItem item = null;

        if(Storage.TryGiveItem(out item, slot, count) == false)
            throw new InvalidOperationException("Seller couldn't return the item");

        return item;
    }
}
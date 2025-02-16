using System;

public class Seller : Trader<ISaleItem>
{
    public ISaleItem SellItem(ISimpleSlot slot, int count)
    {
        if (slot == null)
            throw new ArgumentNullException(nameof(slot));

        ISaleItem item = null;

        if (count == 0)
        {
            if (Storage.TryGetItem(out item, (readOnlySlot) => readOnlySlot.Id == slot.Id) == false)
                throw new InvalidOperationException("Seller couldn't return the item");
        }
        else
        {
            if (Storage.TryGiveItem(out item, slot, count) == false)
                throw new InvalidOperationException("Seller couldn't return the item");
        }

        return item;
    }
}
using System;

public class Buyer : Trader<IObjectItem>
{
    public void BuyItem(ISaleItem item, int count)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (Storage.TryAddItem(item, count) == false)
            throw new InvalidOperationException("The buyer couldn't add the item");
    }
}
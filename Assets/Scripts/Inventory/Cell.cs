using System;

public class Cell : ICellReadOnly
{
    public Cell(Item item, int count, int maxCount)
    {
        Item = item;
        Count = count;
        MaxCount = maxCount;
    }

    public Item Item { get; private set; }
    public int Count { get; private set; }
    public int MaxCount { get; private set; }

    public void AddItem(Item item) => AddItem(item, 1, 1);

    public void AddItem(Item item, int count) => AddItem(item, count, MaxCount);

    public void AddItem(Item item, int count, int maxCount) 
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (count <= 0) 
            throw new ArgumentOutOfRangeException(nameof(count));

        if (maxCount < count)
            throw new ArgumentOutOfRangeException(nameof(maxCount));

        if (Item != item)
        {
            Item = item;
            Count = count;
            MaxCount = maxCount;
            return;
        }

        Count = Math.Clamp(Count + count, 0, MaxCount);
    }
}
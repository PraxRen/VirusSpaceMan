using System;

public interface ISimpleSlot
{
    event Action AddedItem;
    event Action RemovedItem;

    public string Id { get; }
    public int Count { get; }
    public bool IsEmpty { get; }

    public IObjectItem GetItem();
}
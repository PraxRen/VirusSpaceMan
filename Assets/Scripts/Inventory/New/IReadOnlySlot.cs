internal interface IReadOnlySlot<T> where T : Item
{
    public T Item { get; }
    public int Count { get; }

    public T GetItem();
}
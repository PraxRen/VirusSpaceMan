interface IStorage<T> : IReadOnlyStorage<T> where T : IObjectItem
{
    public bool TryAddItem(IReadOnlySlot<T> slot, T item, int count);

    public bool TryAddItem(T item, int count);

    public bool TryRemoveItem(IReadOnlySlot<T> slot, int count);

    public bool TryRemoveItem(T item, int count);

    public bool TryGiveItem(out T item, IReadOnlySlot<T> slot, int count);

    public bool HasItem(T item);
}
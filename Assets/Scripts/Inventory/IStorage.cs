public interface IStorage<T> : IReadOnlyStorage<T> where T : IObjectItem
{
    bool TryAddItem(IReadOnlySlot<T> slot, T item, int count);

    bool TryAddItem(T item, int count);

    bool TryRemoveItem(IReadOnlySlot<T> slot, int count);

    bool TryRemoveItem(T item, int count);

    bool TryGiveItem(out T item, IReadOnlySlot<T> slot, int count);

    bool HasItem(T item);
}
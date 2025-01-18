public interface IReadOnlySlot<T> where T : IObjectItem
{
    public string Id { get; }
    public T Item { get; }
    public int Count { get; }
    public bool IsEmpty { get; }
}
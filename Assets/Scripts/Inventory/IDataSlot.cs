public interface IDataSlot<T> where T : IObjectItem
{
    T Item { get; }
    int Count { get; }
}
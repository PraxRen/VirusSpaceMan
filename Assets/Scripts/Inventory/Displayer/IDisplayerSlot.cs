public interface IDisplayerSlot<T> : IReadOnlyDisplayerSlot<T> where T : IObjectItem
{
    void Initilize(IReadOnlySlot<T> slot);
}
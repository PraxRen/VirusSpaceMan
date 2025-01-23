public interface IDisplayerSlotFactory<T> where T : IObjectItem
{
    IDisplayerSlot<T> Create(IReadOnlySlot<T> slot, IDisplayerStorage<T> displayerStorage);
}
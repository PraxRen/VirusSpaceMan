public interface ISlotFactory<T> where T : IObjectItem
{
    BaseSlot<T> Create(IDataSlot<T> dataSlot, IReadOnlyStorage<T> storage);
}
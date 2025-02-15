public interface ISlotFactory<T> where T : IObjectItem
{
    BaseSlot<T> Create(DataSlot dataSlot, IReadOnlyStorage<T> storage);
}
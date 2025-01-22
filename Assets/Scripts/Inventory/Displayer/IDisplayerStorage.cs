public interface IDisplayerStorage<T> : IReadOnlyDisplayerStorage<T> where T : IObjectItem
{
    void Initilize(IReadOnlyStorage<T> storage, IDisplayerSlotFactory<T> displayerSlotFactory);

    void Next();

    void Previous();
}
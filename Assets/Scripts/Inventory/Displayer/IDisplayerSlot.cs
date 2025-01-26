public interface IDisplayerSlot<T> : IReadOnlyDisplayerSlot<T> where T : IObjectItem
{
    void Initialize(IReadOnlySlot<T> slot);

    void Hide();

    void Show();
}
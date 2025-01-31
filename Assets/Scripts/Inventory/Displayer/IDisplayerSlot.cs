public interface IDisplayerSlot<T> : IReadOnlyDisplayerSlot<T> where T : IObjectItem
{
    void InitializeSlot(IReadOnlySlot<T> slot);

    void Hide();

    void Show();
}
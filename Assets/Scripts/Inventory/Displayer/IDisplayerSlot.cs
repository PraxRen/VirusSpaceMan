public interface IDisplayerSlot : IReadOnlyDisplayerSlot
{
    void InitializeSlot(ISimpleSlot slot);

    void Hide();

    void Show();

    void Destroy();
}
public interface IObjectInteraction : IReadOnlyObjectInteraction
{
    ITarget StartPoint { get; }

    void InteractBefore();

    void Interact();

    void InteractAfter();
}
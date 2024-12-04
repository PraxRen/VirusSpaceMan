public interface IObjectInteraction : IReadOnlyObjectInteraction
{
    void InteractBefore();

    void Interact();

    void InteractAfter();
}
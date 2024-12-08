public interface IObjectInteraction : IReadOnlyObjectInteraction
{
    void StartInteract();

    void InteractBefore();

    void Interact();

    void InteractAfter();

    void StopInteract();
}
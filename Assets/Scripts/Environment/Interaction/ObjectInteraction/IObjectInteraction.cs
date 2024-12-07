public interface IObjectInteraction : IReadOnlyObjectInteraction
{
    bool IsActive { get; }

    void StartInteract();

    void InteractBefore();

    void Interact();

    void InteractAfter();

    void StopInteract();
}
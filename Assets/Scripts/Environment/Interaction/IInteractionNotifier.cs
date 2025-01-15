using System;

public interface IInteractionNotifier
{
    public event Action BeforeInteract;
    public event Action Interacted;
    public event Action AfterInteract;

    bool CanRun();

    void Run();

    void Stop();
}
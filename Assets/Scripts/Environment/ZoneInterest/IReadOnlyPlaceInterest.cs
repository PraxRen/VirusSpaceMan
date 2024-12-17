using System;

public interface IReadOnlyPlaceInterest : ITarget
{
    event Action EnteredInteractor;

    string Name { get; }
    bool IsEmpty { get; }
    bool HasInteractorInside { get; }
    IReadOnlyInteractor Interactor { get; }

    void Reserve(IReadOnlyInteractor interactor);
    bool TryGetObjectInteraction(IReadOnlyInteractor interactor, out IObjectInteraction objectInteraction);
}
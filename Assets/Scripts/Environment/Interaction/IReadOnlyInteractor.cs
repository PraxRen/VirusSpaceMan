using System;
using UnityEngine;

public interface IReadOnlyInteractor : ITarget
{
    event Action<IReadOnlyInteractor, IReadOnlyObjectInteraction> StartedInteract;
    event Action<IReadOnlyInteractor, IReadOnlyObjectInteraction> Interacted;
    event Action<IReadOnlyInteractor, IReadOnlyObjectInteraction> StoppedInteract;

    bool IsActive { get; }
    IReadOnlyObjectInteraction ObjectInteraction { get; }
    LayerMask LayerObjectInteraction {  get; }
}
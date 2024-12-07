using System;
using UnityEngine;

public interface IReadOnlyInteractor
{
    event Action<IReadOnlyObjectInteraction> StartedInteract;
    event Action<IReadOnlyObjectInteraction> Interacted;
    event Action<IReadOnlyObjectInteraction> StoppedInteract;

    bool IsActive { get; }
    IReadOnlyObjectInteraction ObjectInteraction { get; }
    LayerMask LayerObjectInteraction {  get; }
}
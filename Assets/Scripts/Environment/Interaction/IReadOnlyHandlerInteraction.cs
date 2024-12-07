using System;
using UnityEngine;

public interface IReadOnlyHandlerInteraction
{
    event Action<IReadOnlyObjectInteraction> StartedInteract;
    event Action<IReadOnlyObjectInteraction> Interacted;
    event Action<IReadOnlyObjectInteraction> StoppedInteract;

    bool IsActive { get; }
    IReadOnlyObjectInteraction ObjectInteraction { get; }
    LayerMask LayerObjectInteraction {  get; }
}
using System;
using UnityEngine;

public interface IReadOnlyTrigger
{
    event Action<Collider> BeforeChangedTarget;
    event Action<Collider> ChangedTarget;
    event Action RemovedTarget;

    Collider Target { get; }
}
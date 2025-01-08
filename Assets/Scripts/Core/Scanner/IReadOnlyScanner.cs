using System.Collections.Generic;
using System;
using UnityEngine;

public interface IReadOnlyScanner
{
    event Action<IReadOnlyCollection<Collider>> ChangedTargets;
    event Action<Collider> BeforeChangedCurrentTarget;
    event Action<Collider> ChangedCurrentTarget;
    event Action<Collider> RemovedCurrentTarget;
    event Action<float> ChangedRadius;

    Collider Target { get; }
    float Radius { get; }
}
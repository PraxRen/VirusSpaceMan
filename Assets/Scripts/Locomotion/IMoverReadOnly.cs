using System;
using UnityEngine;

public interface IMoverReadOnly
{
    event Action StepTook;

    Vector3 Velocity { get; }
    LayerMask GroundLayer { get; }
}
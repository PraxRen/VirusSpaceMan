using System;
using UnityEngine;

public class SimpleEvent
{
    public SimpleEvent(TypeSimpleEvent type, LayerMask layerMask, float radius)
    {
        if (radius <= 0)
            throw new ArgumentOutOfRangeException(nameof(radius));

        Type = type;
        LayerMask = layerMask;
        Radius = radius;
    }

    public TypeSimpleEvent Type { get; }
    public LayerMask LayerMask { get; }
    public float Radius { get; }
}
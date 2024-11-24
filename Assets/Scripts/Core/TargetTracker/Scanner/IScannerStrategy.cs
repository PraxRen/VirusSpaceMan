using System.Collections.Generic;
using UnityEngine;

public interface IScannerStrategy
{
    IEnumerable<Collider> Sort(IEnumerable<Collider> targets, Transform transform);
}
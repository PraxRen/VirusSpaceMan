using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReadOnlyHandlerInteraction
{
    IReadOnlyObjectInteraction ObjectInteraction { get; }
}

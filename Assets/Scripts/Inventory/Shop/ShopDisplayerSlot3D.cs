using System;
using UnityEngine;

public class ShopDisplayerSlot3D : MonoBehaviour, IDisplayerSlot<ISaleItem>
{
    public IReadOnlySlot<ISaleItem> Slot { get; private set; }

    public event Action<IDisplayerSlot<ISaleItem>> Selected;

    public void Initilize(IReadOnlySlot<ISaleItem> slot)
    {
        if (slot == null)
            throw new ArgumentNullException(nameof(slot));

        Slot = slot;
    }
}

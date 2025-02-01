using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISimpleStorage
{
    IReadOnlyList<ISimpleSlot> GetSlots();
}
using System;
using System.Collections.Generic;

public interface ISimpleStorage
{
    event Action Initialized;

    IReadOnlyList<ISimpleSlot> GetSlots();
}
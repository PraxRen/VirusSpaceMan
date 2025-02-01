using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISimpleSlot
{
    public string Id { get; }
    public int Count { get; }
    public bool IsEmpty { get; }

    public IObjectItem GetItem();
}
using System;
using UnityEngine;

public class Communication : MonoBehaviour, IReadOnlyCommunication
{
    public event Action<TypeCommunication> Started;

    public void Run(TypeCommunication type)
    {
        Started?.Invoke(type);
        Debug.Log($"!!!{transform.parent.name}!!!");
    }
}
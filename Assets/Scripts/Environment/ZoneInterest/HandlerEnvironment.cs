using System;
using UnityEngine;

public class HandlerEnvironment : MonoBehaviour, IReadOnlyHandlerEnvironment
{
    [SerializeField] private Environment _defaultZone;

    public event Action<Environment> ChangedZone;

    public Environment CurrentZone { get; private set; }

    private void Awake()
    {
        SetZone(_defaultZone);
    }

    public void SetZone(Environment zone)
    {
        if (zone == CurrentZone)
            return;

        CurrentZone = zone;
        ChangedZone?.Invoke(zone);
    }
}
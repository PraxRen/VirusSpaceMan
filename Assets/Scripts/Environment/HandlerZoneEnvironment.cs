using System;
using UnityEngine;

public class HandlerZoneEnvironment : MonoBehaviour, IReadOnlyHandlerZoneEnvironment
{
    [SerializeField] private ZoneEnvironment _defaultZone;

    public event Action<ZoneEnvironment> ChangedZone;

    public ZoneEnvironment CurrentZone { get; private set; }

    private void Awake()
    {
        SetZone(_defaultZone);
    }

    public void SetZone(ZoneEnvironment zone)
    {
        if (zone == CurrentZone)
            return;

        CurrentZone = zone;
        ChangedZone?.Invoke(zone);
    }
}
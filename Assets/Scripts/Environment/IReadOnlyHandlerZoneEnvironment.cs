using System;

public interface IReadOnlyHandlerZoneEnvironment
{
    event Action<ZoneEnvironment> ChangedZone;
    ZoneEnvironment CurrentZone { get; }
}
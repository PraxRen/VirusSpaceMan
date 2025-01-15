using System;

public interface IReadOnlyHandlerEnvironment
{
    event Action<Environment> ChangedZone;
    Environment CurrentZone { get; }
}
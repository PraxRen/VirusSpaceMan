using System;

public interface IReadOnlyActivatorRagdoll
{
    event Action<Hit> BeforeActivated;
    event Action<Hit> Activated;
    event Action Deactivated;
}
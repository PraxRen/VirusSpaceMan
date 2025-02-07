using System;

public interface IReadOnlyShop
{
    event Action Activated;
    event Action Deactivated;

    void Activate();
    void Deactivate();
}

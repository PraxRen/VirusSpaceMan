using System;

public interface IReadOnlyShop
{
    event Action<ISimpleSlot> BeforeChangedActiveSlot;
    event Action<ISimpleSlot> ChangedActiveSlot;
    event Action<IReadOnlyTrader, IReadOnlyTrader> Initialized;
    event Action Activated;
    event Action Deactivated;
    event Action Emptied;

    public ISimpleSlot ActiveSlot { get; }

    void Activate();

    void Deactivate();
}
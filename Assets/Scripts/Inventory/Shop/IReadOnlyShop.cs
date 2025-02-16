using System;
using static Shop;

public interface IReadOnlyShop
{
    event Action<ShopActiveSlot> BeforeChangedActiveSlot;
    event Action<ShopActiveSlot> ChangedActiveSlot;
    event Action<IReadOnlyTrader, IReadOnlyTrader> Initialized;
    event Action Activated;
    event Action Deactivated;
    event Action Emptied;

    public ShopActiveSlot ActiveSlot { get; }

    void Activate();

    void Deactivate();
}
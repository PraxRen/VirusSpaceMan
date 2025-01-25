using System;

public class EquipmentSlot : BaseSlot<IEquipmentItem>, IReadOnlyEquipmentSlot
{
    public EquipmentSlot(EquipmentType type) : base()
    {
        Type = type;
    }

    public EquipmentSlot(IEquipmentItem item, int count)
    {
        Type = item.Type;
        TryAddItemAddon(item, count);
    }

    public EquipmentType Type { get; }

    public override bool TryGiveItem(out IEquipmentItem item, int count)
    {
        if (count != 1)
            throw new ArgumentOutOfRangeException(nameof(count));

        item = default;

        if (IsEmpty)
            return false;

        item = Item;

        if (TryRemoveItem(count) == false)
        {
            item = default;
            return false;
        }

        return true;
    }

    protected override bool TryAddItemAddon(IEquipmentItem item, int count)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (count != 1)
            throw new ArgumentOutOfRangeException(nameof(count));

        if (IsEmpty == false)
            throw new InvalidOperationException($"Ñannot add an {nameof(IEquipmentItem)} to an occupied {nameof(EquipmentSlot)}");

        if (Type != item.Type)
            throw new InvalidOperationException($"Type mismatch");

        Item = item;
        Count = 1;
        return true;
    }

    protected override bool TryRemoveItemAddon(int count)
    {
        if (count != 1)
            throw new ArgumentOutOfRangeException(nameof(count));

        if (IsEmpty)
            return false;

        Item = default;
        Count = 0;
        return true;
    }
}
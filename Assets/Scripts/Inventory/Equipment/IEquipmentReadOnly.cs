using System;

public interface IEquipmentReadOnly
{
    event Action<IEquipmentCellReanOnly> ChangedCell;

    Item GetItem(EquipmentType type);
}

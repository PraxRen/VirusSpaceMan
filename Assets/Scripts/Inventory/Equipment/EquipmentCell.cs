public class EquipmentCell : Cell, IEquipmentCellReanOnly
{
    public EquipmentCell(EquipmentType type, Item item) : base(item, 1, 1)
    {
        Type = type;
    }

    public EquipmentType Type { get; private set; }
}
public interface IDataEquipmentSlot : IDataSlot<IEquipmentItem>
{
    public EquipmentType Type { get; }
}   
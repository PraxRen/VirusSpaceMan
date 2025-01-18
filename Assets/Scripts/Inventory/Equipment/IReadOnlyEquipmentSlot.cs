public interface IReadOnlyEquipmentSlot : IReadOnlySlot<IEquipmentItem>  
{
    public EquipmentType Type { get; }
}
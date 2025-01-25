using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDefaultEquipmentSlots", menuName = "Inventory/Equipment/DefaultEquipmentSlots")]
public class DefaultEquipmentSlots : ScriptableObject
{
    [SerializeField] private DataEquipmentSlot[] _values;

    public IReadOnlyCollection<DataEquipmentSlot> Values => _values;

}
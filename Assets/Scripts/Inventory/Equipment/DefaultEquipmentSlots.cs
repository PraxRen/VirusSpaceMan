using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDefaultEquipmentSlots", menuName = "Inventory/Equipment/DefaultEquipmentSlots")]
public class DefaultEquipmentSlots : ScriptableObject
{
    [SerializeField] private SettingEquipmentSlot[] _slots;

    public IReadOnlyCollection<SettingEquipmentSlot> Slots => _slots;

    [System.Serializable]
    public class SettingEquipmentSlot
    {
        [SerializeField] public EquipmentType _type;
        [SerializeField][SerializeInterface(typeof(IEquipmentItem))] private ScriptableObject _itemScriptableObject;

        public EquipmentType Type => _type;
        public IEquipmentItem Item => (IEquipmentItem)_itemScriptableObject;
    }
}
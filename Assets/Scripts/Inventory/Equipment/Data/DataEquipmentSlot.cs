using UnityEngine;

[System.Serializable]
public class DataEquipmentSlot : IDataEquipmentSlot
{
    [SerializeField] public EquipmentType _type;
    [SerializeField][SerializeInterface(typeof(IEquipmentItem))] private ScriptableObject _itemScriptableObject;
    [SerializeField] private int _count;

    public EquipmentType Type => _type;
    public IEquipmentItem Item => (IEquipmentItem)_itemScriptableObject;
    public int Count => _count;
}
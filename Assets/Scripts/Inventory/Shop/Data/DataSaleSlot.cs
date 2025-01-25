using UnityEngine;

[System.Serializable]
public class DataSaleSlot : IDataSlot<ISaleItem>
{
    [SerializeField][SerializeInterface(typeof(ISaleItem))] private ScriptableObject _itemScriptableObject;
    [SerializeField] private int _count;

    public ISaleItem Item => (ISaleItem)_itemScriptableObject;

    public int Count => _count;
}
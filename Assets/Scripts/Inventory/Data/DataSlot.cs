using UnityEngine;

[System.Serializable]
public class DataSlot : IDataSlot<IObjectItem>
{
    [SerializeField][SerializeInterface(typeof(IObjectItem))] private ScriptableObject _itemScriptableObject;
    [SerializeField] private int _count;

    public IObjectItem Item => (IObjectItem)_itemScriptableObject;

    public int Count => _count;
}